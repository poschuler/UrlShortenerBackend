using Microsoft.AspNetCore.Routing;
using UrlShortener.Api.Shared.Endpoints;
using UrlShortener.Api.Shared.Exceptions;
using UrlShortener.Api.Shared.Extensions;
using UrlShortener.Api.Shared.Messaging;

namespace UrlShortener.Api.Feature.Urls.ShortenUrl
{
    public class ShortenUrlEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("urls",
               async (
               ShortenUrlCommand command,
               ICommandHandler<ShortenUrlCommand, ShortenUrlResponse> handler,
               CancellationToken cancellationToken
               ) =>
               {
                   var result = await handler.Handle(command, cancellationToken);
                   return result.Match(Results.Ok, CustomResults.Problem);
               })
           .WithTags(Tags.Url);

        }
    }
}
