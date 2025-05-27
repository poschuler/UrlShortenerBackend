using UrlShortener.Api.Shared.Domain.Models;

namespace UrlShortener.Api.Shared.Networking
{
    public interface ITokenRangeGeneratorApiClient
    {
        Task<TokenRange> GenerateTokenRangeAsync(
            long size,
            string requestServerId,
            CancellationToken cancellationToken = default);
    }
}
