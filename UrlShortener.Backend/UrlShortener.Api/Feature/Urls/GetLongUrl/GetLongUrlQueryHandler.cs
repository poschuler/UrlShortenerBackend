using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Shared.Base;
using UrlShortener.Api.Shared.Messaging;
using UrlShortener.Api.Shared.Persistence;

namespace UrlShortener.Api.Feature.Urls.GetLongUrl
{
    public class GetLongUrlQueryHandler(UrlShortenerDbContext dbContext) : IQueryHandler<GetLongUrlQuery>
    {
        public async Task<Result> Handle(GetLongUrlQuery query, CancellationToken cancellationToken)
        {
            var shortenUrl = await
                    dbContext.ShortenedUrls
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ShortCode == query.ShortCode, cancellationToken);

            if (shortenUrl == null)
            {
                return Result.Failure(Error.NotFound("UrlShortener.UrlNotFound", "The requested shortened URL does not exist."));
            }

            return Result.Redirect(shortenUrl.LongUrl, true);
        }
    }
}
