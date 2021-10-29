namespace MASA.EShop.Api.Caller
{
    public interface IDaprClientCaller : ICaller, IDisposable
    {
        public string? AppId { get; init; }
    }
}
