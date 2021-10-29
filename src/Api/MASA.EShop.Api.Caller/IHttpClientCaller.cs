using System.Text.Json;

namespace MASA.EShop.Api.Caller
{
    public interface IHttpClientCaller : ICaller, IDisposable
    {
        public string? BaseAddress { get; set; }

        Task<TValue?> GetFromJsonAsync<TValue>(string? requestUri, JsonSerializerOptions? options, CancellationToken cancellationToken = default(CancellationToken));

        Task<TValue?> GetFromJsonAsync<TValue>(string? requestUri, CancellationToken cancellationToken = default(CancellationToken));

        Task<HttpResponseMessage> PostAsJsonAsync<TValue>(string? requestUri, TValue value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken));

        Task<string> GetStringAsync(string? requestUri, CancellationToken cancellationToken);

        Task<HttpResponseMessage> PutAsync(string? requestUri, HttpContent? content);
    }
}
