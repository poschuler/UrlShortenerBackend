
namespace UrlShortener.Api.Shared.TokenManager
{
    public class TokenProvisioningBackgroundService : BackgroundService
    {
        private readonly ITokenManagerService _tokenManagerService;
        private readonly ILogger<TokenProvisioningBackgroundService> _logger;
        private readonly TimeSpan _provisioningInterval;

        public TokenProvisioningBackgroundService(
            ITokenManagerService tokenManagerService,
            ILogger<TokenProvisioningBackgroundService> logger,
            IConfiguration configuration)
        {
            _tokenManagerService = tokenManagerService;
            _logger = logger;
            var intervalSeconds = configuration.GetValue<int>("TokenManagerService:ProvisionIntervalSeconds");
            _provisioningInterval = TimeSpan.FromSeconds(intervalSeconds);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Token provisioning background service started.");

            await _tokenManagerService.ProvisionTokensIfNeededAsync(cancellationToken);

            var timer = new PeriodicTimer(_provisioningInterval);

            try
            {
                while (await timer.WaitForNextTickAsync(cancellationToken))
                {
                    try
                    {
                        await _tokenManagerService.ProvisionTokensIfNeededAsync(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error provisioning tokens in background.");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Token provisioning background service is stopping.");
            }
            finally
            {
                timer.Dispose();
                _logger.LogInformation("Token provisioning background service stopped.");
            }

        }
    }
}
