
using System.Collections.Concurrent;
using System.Net;
using UrlShortener.Api.Shared.Networking;

namespace UrlShortener.Api.Shared.TokenManager
{
    public class TokenManagerService : ITokenManagerService
    {
        private readonly ILogger<TokenManagerService> _logger;
        private readonly ITokenRangeGeneratorApiClient _tokenRangeGeneratorApiClient;
        private readonly ConcurrentQueue<long> _tokens = new();
        private static readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly int _minThreshold;
        private readonly int _rangeToFetch;
        private readonly string _serverId;


        public TokenManagerService(
            ILogger<TokenManagerService> logger,
            ITokenRangeGeneratorApiClient tokenRangeGeneratorApiClient,
            IConfiguration configuration)
        {
            _logger = logger;
            _tokenRangeGeneratorApiClient = tokenRangeGeneratorApiClient;
            _rangeToFetch = configuration.GetValue<int>("TokenManagerService:TokenCount");
            _minThreshold = configuration.GetValue<int>("TokenManagerService:MinThreshold");
            _serverId = Dns.GetHostName();

        }

        public int AvailableTokens => _tokens.Count;

        public async Task ProvisionTokensIfNeededAsync(CancellationToken cancellationToken = default)
        {

            await _semaphore.WaitAsync(cancellationToken);
            try
            {

                if (AvailableTokens < _minThreshold)
                {
                    _logger.LogInformation("El número de tokens ({AvailableTokens}) está por debajo del umbral ({MinThreshold}). Solicitando nuevos tokens.", AvailableTokens, _minThreshold);

                    try
                    {
                        var newTokens = await _tokenRangeGeneratorApiClient.GenerateTokenRangeAsync(_rangeToFetch, _serverId, cancellationToken);
                        if (newTokens is not null)
                        {
                            for (var i = newTokens.RangeStart; i <= newTokens.RangeEnd; i++)
                            {
                                _tokens.Enqueue(i);
                            }
                            _logger.LogInformation("Se agregaron tokens del {Start} al {End} (Total: {Count}). Tokens ahora: {AvailableTokens}", newTokens.RangeStart, newTokens.RangeEnd, newTokens.RangeEnd - newTokens.RangeStart + 1, AvailableTokens);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error al intentar obtener un nuevo rango de tokens de la API externa.");
                    }

                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public bool TryGetToken(out long token)
        {
            if (_tokens.TryDequeue(out token))
            {
                _logger.LogInformation("Token {Token} dequeued successfully.", token);
                //_ = ProvisionTokensIfNeededAsync();
                return true;
            }

            _logger.LogWarning("No tokens available to dequeue.");
            token = 0;
            return false;
        }
    }
}
