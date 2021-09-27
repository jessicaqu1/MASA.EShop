
namespace MASA.EShop.Services.Basket.Infrastructure.Events
{
    public class CheckoutAcceptedIntegrationEvent : IntegrationEvent
    {
        public override string Topic { get ; set ; }= nameof(CheckoutAcceptedIntegrationEvent);
    }
}
