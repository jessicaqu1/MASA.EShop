namespace MASA.EShop.Api.Caller;

public interface IDaprClientCaller : ICaller
{
    public string? AppId { get; init; }
}

