namespace MASA.EShop.Services.Ordering.Actors
{
    public interface IOrderingProcessActor : IActor
    {
        Task<bool> Cancel();

        Task<bool> Ship();

        Task NotifyPaymentSucceeded();

        Task NotifyPaymentFailed();
    }
}
