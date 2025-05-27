using FluentValidation;

namespace UrlShortener.Api.Feature.Urls.ShortenUrl
{
    public class ShortenUrlValidator : AbstractValidator<ShortenUrlCommand>
    {
        public ShortenUrlValidator()
        {
            RuleFor(c => c.LongUrl)
                .NotEmpty()
                .MaximumLength(2048)
                .Must(BeAValidUrl).WithMessage("Invalid URL format. Must be a valid absolute URL starting with http:// or https://");
        }

        private bool BeAValidUrl(string url)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out var uriResult))
            {
                return uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps;
            }

            return false;
        }
    }
}
