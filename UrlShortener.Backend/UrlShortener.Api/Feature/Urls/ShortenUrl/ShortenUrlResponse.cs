namespace UrlShortener.Api.Feature.Urls.ShortenUrl
{
    public sealed class ShortenUrlResponse
    {
        public string LongUrl { get; set; } = string.Empty;
        public string ShortCode { get; set; } = string.Empty;
    }
}
