namespace MASA.EShop.Api.Caller;

public interface IHttpClientCaller : ICaller
{
    public string? BaseAddress { get; set; }
}

