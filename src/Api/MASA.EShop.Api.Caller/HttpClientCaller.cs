using System.Net.Http.Json;
using System.Text.Json;

namespace MASA.EShop.Api.Caller
{
    public class HttpClientCaller : ICaller
    {
        private readonly HttpClient _httpClient;

        public Uri? BaseAddress { get; set; }

        public HttpClientCaller(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<TValue?> GetFromJsonAsync<TValue>(string? requestUri, JsonSerializerOptions? options, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetFromJsonAsync<TValue>(requestUri, options, cancellationToken);
        }

        public async Task<string> GetStringAsync(string? requestUri, CancellationToken cancellationToken)
        {
            return await _httpClient.GetStringAsync(requestUri, cancellationToken);
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<TValue>(string? requestUri, TValue value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        {
            return await _httpClient.PostAsJsonAsync<TValue>(requestUri, value, options, cancellationToken);
        }
    }
}
