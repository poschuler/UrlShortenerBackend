namespace UrlShortener.Api.Shared.Domain.Entities
{
    public sealed class ShortenedUrl
    {
        public string LongUrl { get; set; } = string.Empty;
        public string ShortCode { get; set; } = string.Empty;
        public DateTime CreatedOnUtc { get; set; }
    }
}
