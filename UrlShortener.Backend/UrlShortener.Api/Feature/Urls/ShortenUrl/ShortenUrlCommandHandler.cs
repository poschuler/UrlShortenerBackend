
using UrlShortener.Api.Shared.Base;
using UrlShortener.Api.Shared.Domain.Entities;
using UrlShortener.Api.Shared.Encoder;
using UrlShortener.Api.Shared.Messaging;
using UrlShortener.Api.Shared.Persistence;
using UrlShortener.Api.Shared.TokenManager;

namespace UrlShortener.Api.Feature.Urls.ShortenUrl
{
    public sealed class ShortenUrlCommandHandler(
        UrlShortenerDbContext dbContext,
        ITokenManagerService tokenManagerService,
        IBase62Converter converter
        ) : ICommandHandler<ShortenUrlCommand, ShortenUrlResponse>
    {
        public async Task<Result<ShortenUrlResponse>> Handle(ShortenUrlCommand command, CancellationToken cancellationToken)
        {

            if (!tokenManagerService.TryGetToken(out long token))
            {
                return Result.Failure<ShortenUrlResponse>(Error.Problem("TokenManager.Unavailable", "Unable to generate a token for URL shortening. Please try again later."));
            }

            var shortCode = converter.Encode(token);

            var shortenUrlObject = new ShortenedUrl
            {
                LongUrl = command.LongUrl,
                ShortCode = shortCode
            };

            dbContext.ShortenedUrls.Add(shortenUrlObject);
            await dbContext.SaveChangesAsync(cancellationToken);

            var response = new ShortenUrlResponse
            {
                LongUrl = shortenUrlObject.LongUrl,
                ShortCode = shortCode
            };

            return response;
        }
    }
}
