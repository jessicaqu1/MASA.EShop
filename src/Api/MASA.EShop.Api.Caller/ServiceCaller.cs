using Dapr.Client;
using Google.Api;
using System.Net.Http.Json;
using System.Text.Json;

namespace MASA.EShop.Api.Caller
{
    public abstract class ServiceCaller : IHttpClientCaller,IDaprClientCaller,ICaller
    {
        private DaprClient _daprClient { get; set; }

        private HttpClient _httpClient { get; set; }

        public virtual CallerModes Mode { get; set; } = CallerModes.OriginalHttp;
        public virtual string? BaseAddress { get; set; } 
        public virtual string? AppId { get ; init; }

        public ServiceCaller(HttpClient httpClient,DaprClient daprClient)
        {
            _daprClient = daprClient;
            _httpClient = httpClient;
            if (Mode == CallerModes.OriginalHttp || Mode == CallerModes.OriginalGrpc)
            {
                if (string.IsNullOrEmpty(BaseAddress))
                {
                    throw new($"Original caller mode {nameof(BaseAddress)} must be a value");
                }
                _httpClient.BaseAddress = new Uri(BaseAddress);
            }
            else
            {
                if (string.IsNullOrEmpty(AppId))
                {
                    throw new($"Dapr caller mode {nameof(AppId)} must be a value");
                }
            }
        }



        public void Dispose()
        {
            
        }

        public async Task<TValue?> GetFromJsonAsync<TValue>(string? requestUri, CancellationToken cancellationToken = default)
        {
            return await GetFromJsonAsync<TValue>(requestUri, null,cancellationToken);
        }

        public async Task<TValue?> GetFromJsonAsync<TValue>(string? requestUri, JsonSerializerOptions? options, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetFromJsonAsync<TValue>(requestUri, options, cancellationToken);
        }

        public async Task<string> GetStringAsync(string? requestUri, CancellationToken cancellationToken)
        {
            //await _daprClient.InvokeMethodAsync(HttpMethod.Get, AppId, "", cancellationToken);
            return await _httpClient.GetStringAsync(requestUri, cancellationToken);
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<TValue>(string? requestUri, TValue value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        {
            return await _httpClient.PostAsJsonAsync<TValue>(requestUri, value, options, cancellationToken);
        }

        public async Task<HttpResponseMessage> PutAsync(string? requestUri, HttpContent? content)
        {
            return await _httpClient.PutAsync(requestUri, content);
        }
    }

    public enum CallerModes
    {
        OriginalHttp,
        OriginalGrpc,
        DaprHttp,
        DaprGrpc
    }
}
