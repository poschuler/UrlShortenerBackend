using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Shared.Behaviors;
using UrlShortener.Api.Shared.Encoder;
using UrlShortener.Api.Shared.Exceptions;
using UrlShortener.Api.Shared.Messaging;
using UrlShortener.Api.Shared.Networking;
using UrlShortener.Api.Shared.Persistence;
using UrlShortener.Api.Shared.TokenManager;

namespace UrlShortener.Api.Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UrlShortenerDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("UrlShortenerPG")));
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistenceServices(configuration);

            services.AddHttpClient();

            services.AddSingleton<IBase62Converter, Base62Converter>();
            services.AddSingleton<ITokenRangeGeneratorApiClient, TokenRangeGeneratorApiClient>();
            services.AddSingleton<ITokenManagerService, TokenManagerService>();
            services.AddHostedService<TokenProvisioningBackgroundService>();

            services.Scan(scan => scan.FromAssembliesOf(typeof(ServiceCollectionExtensions))
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator.CommandHandler<,>));
            //services.Decorate(typeof(ICommandHandler<>), typeof(ValidationDecorator.CommandBaseHandler<>));

            //services.Decorate(typeof(IQueryHandler<,>), typeof(LoggingDecorator.QueryHandler<,>));
            //services.Decorate(typeof(IQueryHandler<,>), typeof(LoggingDecorator.QueryHandler<,>));
            //services.Decorate(typeof(ICommandHandler<,>), typeof(LoggingDecorator.CommandHandler<,>));
            //services.Decorate(typeof(ICommandHandler<>), typeof(LoggingDecorator.CommandBaseHandler<>));

            //services.Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
            //    .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
            //    .AsImplementedInterfaces()
            //    .WithScopedLifetime());

            services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly, includeInternalTypes: true);

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
            return services;
        }
    }
}
