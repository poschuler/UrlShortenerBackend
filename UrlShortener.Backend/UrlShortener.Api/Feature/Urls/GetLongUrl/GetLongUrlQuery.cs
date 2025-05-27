using UrlShortener.Api.Shared.Messaging;

namespace UrlShortener.Api.Feature.Urls.GetLongUrl
{
    public sealed record GetLongUrlQuery(string ShortCode) : IQuery;
}
