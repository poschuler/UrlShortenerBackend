using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UrlShortener.Api.Shared.Domain.Models;

namespace UrlShortener.Api.Shared.Networking
{
    public class TokenRangeGeneratorApiClient : ITokenRangeGeneratorApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        //private readonly AsyncRetryPolicy _retryPolicy = Policy
        //    .Handle<HttpRequestException>()
        //    .WaitAndRetryAsync(3, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)));

        public TokenRangeGeneratorApiClient(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }


        private readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerOptions.Web);

        public async Task<TokenRange> GenerateTokenRangeAsync(long size, string requestServerId, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_configuration["Integrations:TokenRangeGeneratorApiRoot"]}token-range");

            request.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var requestBody = new { size, requestServerId };
            var requestBodyJson = JsonSerializer.Serialize(requestBody, _jsonSerializerOptions);
            request.Content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<TokenRange>(stream, _jsonSerializerOptions)
                ?? throw new JsonException("Failed to deserialize TokenRange response.");
        }
    }
}
