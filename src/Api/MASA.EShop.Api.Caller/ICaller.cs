namespace MASA.EShop.Api.Caller;

public interface ICaller : IDisposable
{
    public CallerModes Mode { get; set; }
}

