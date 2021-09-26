using MASA.BuildingBlocks.Data.Uow;
using MASA.Contrib.Dispatcher.IntegrationEvents.Dapr;

namespace MASA.EShop.Services.Basket.Infrastructure.Events
{
    public class CheckoutAcceptedIntegrationEvent : IntegrationEvent
    {
        public override string Topic { get ; set ; }= nameof(CheckoutAcceptedIntegrationEvent);
    }
}
