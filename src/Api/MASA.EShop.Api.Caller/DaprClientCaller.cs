using Dapr.Client;
using System.Net.Http.Json;
using System.Text.Json;

namespace MASA.EShop.Api.Caller
{
    public class DaprClientCaller : IDaprClientCaller
    {
        //https://docs.microsoft.com/zh-cn/dotnet/architecture/dapr-for-net-developers/service-invocation
        private readonly DaprClient _daprClient;
        private readonly HttpClient _httpClient;

        public string AppId { get; set; }

        public DaprClientCaller(DaprClient daprClient)
        {
            _daprClient = daprClient;
            _httpClient = DaprClient.CreateInvokeHttpClient();
        }

        public void Dispose()
        {
            _daprClient.Dispose();
        }

        public async Task<TValue?> GetFromJsonAsync<TValue>(string? requestUri, JsonSerializerOptions? options, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetFromJsonAsync<TValue>(requestUri, options, cancellationToken);
        }

        public async Task<string> GetStringAsync(string? requestUri, CancellationToken cancellationToken)
        {
            await _daprClient.InvokeMethodAsync(HttpMethod.Get, AppId, "", cancellationToken);
            return await _httpClient.GetStringAsync(requestUri, cancellationToken);
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<TValue>(string? requestUri, TValue value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        {
            return await _httpClient.PostAsJsonAsync<TValue>(requestUri, value, options, cancellationToken);
        }
    }
}
