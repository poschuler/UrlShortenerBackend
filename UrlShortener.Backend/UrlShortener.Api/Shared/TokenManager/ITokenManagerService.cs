namespace UrlShortener.Api.Shared.TokenManager
{
    public interface ITokenManagerService
    {
        bool TryGetToken(out long token);
        Task ProvisionTokensIfNeededAsync(CancellationToken cancellationToken = default);
        int AvailableTokens { get; }
    }
}
