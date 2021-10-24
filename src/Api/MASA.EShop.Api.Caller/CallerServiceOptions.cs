namespace MASA.EShop.Api.Caller
{
    public class CallerServiceOptions
    {

        private Dictionary<string, string> _dicBaseAddress = new();
        private Dictionary<string, string> _dicAppId = new();

        public void SetBaseAddress<T>(T TValue, string baseAddress) where T : HttpClientCaller
        {
            _dicBaseAddress.TryAdd(TValue.GetHashCode().ToString(), baseAddress);
        }

        public void SetAppId<T>(T TValue, string appId) where T : DaprClientCaller
        {
            _dicAppId.TryAdd(TValue.GetHashCode().ToString(), appId);
        }
    }
}
