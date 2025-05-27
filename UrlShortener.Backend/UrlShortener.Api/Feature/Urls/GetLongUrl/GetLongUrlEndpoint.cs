using Microsoft.AspNetCore.Routing;
using UrlShortener.Api.Feature.Urls.ShortenUrl;
using UrlShortener.Api.Shared.Domain.Entities;
using UrlShortener.Api.Shared.Endpoints;
using UrlShortener.Api.Shared.Exceptions;
using UrlShortener.Api.Shared.Extensions;
using UrlShortener.Api.Shared.Messaging;

namespace UrlShortener.Api.Feature.Urls.GetLongUrl
{
    public class GetLongUrlEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet(
                "urls/{shortCode}",
                async (
                    string shortCode,
                    IQueryHandler<GetLongUrlQuery> handler,
                    CancellationToken cancellationToken
                    ) =>
                {
                    var query = new GetLongUrlQuery(shortCode);

                    var result = await handler.Handle(query, cancellationToken);


                    return result.MatchOrRedirect(
                        CustomResults.Problem,
                        (url, permanent) => Results.Redirect(url, permanent));

                }).WithTags(Tags.Url);
        }
    }

}
