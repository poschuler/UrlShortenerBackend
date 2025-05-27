using UrlShortener.Api.Shared.Messaging;

namespace UrlShortener.Api.Feature.Urls.ShortenUrl
{
    public sealed record ShortenUrlCommand(string LongUrl) : ICommand<ShortenUrlResponse>;

}
