using TokenRangeGenerator.Api.Shared.Endpoints;
using TokenRangeGenerator.Api.Shared.Exceptions;
using TokenRangeGenerator.Api.Shared.Extensions;
using TokenRangeGenerator.Api.Shared.Messaging;

namespace TokenRangeGenerator.Api.Features.TokenRanges.Generate
{
    public class GenerateTokenRangeEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/token-range",
               async (
               GenerateTokenRangeCommand command,
               ICommandHandler<GenerateTokenRangeCommand, GenerateTokenRangeResponse> handler,
               CancellationToken cancellationToken
               ) =>
               {
                   var result = await handler.Handle(command, cancellationToken);
                   return result.Match(Results.Ok, CustomResults.Problem);
               })
           .WithTags(Tags.TokenRange);
        }
    }
}
