using Microsoft.Extensions.DependencyInjection;

namespace MASA.EShop.Api.Caller
{
    public class CallerServiceOptions
    {

        private Dictionary<string, string> _dicBaseAddress = new();
        private Dictionary<string, string> _dicAppId = new();

        public IHttpClientBuilder SetBaseAddress<T>(string baseAddress) where T : HttpClientCaller
        {
            _dicBaseAddress.TryAdd(typeof(T).Name, baseAddress);
            var builder = new CallerHttpClientBuilder(services, typeof(T).Name);
            return builder;
        }

        public CallerServiceOptions SetAppId<T>(string appId) where T : DaprClientCaller
        {
            _dicAppId.TryAdd(typeof(T).Name, appId);
            return this;
        }

        public (bool ok, string baseAddress) GetBaseAddress(string name)
        {
            var ok = _dicBaseAddress.TryGetValue(name, out var baseAddress);
            return (ok, baseAddress ?? "");
        }

        public (bool ok, string appId) GetAppId(string name)
        {
            var ok = _dicAppId.TryGetValue(name, out var appId);
            return (ok, appId ?? "");
        }
    }
}
