using MASA.Contrib.Dispatcher.IntegrationEvents.Dapr;

namespace MASA.EShop.Contracts.Basket
{
    public class OrderStatusChangedToSubmittedIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; set; }
        public string OrderStatus { get; set; } = default!;
        public string BuyerId { get; set; } = default!;
        public string BuyerName { get; set; } = default!;
        public override string Topic { get ; set ; } = nameof(OrderStatusChangedToSubmittedIntegrationEvent);

        public OrderStatusChangedToSubmittedIntegrationEvent()
        {
        }

        public OrderStatusChangedToSubmittedIntegrationEvent(Guid orderId, string orderStatus,
            string buyerId, string buyerName)
        {
            OrderId = orderId;
            OrderStatus = orderStatus;
            BuyerId = buyerId;
            BuyerName = buyerName;
        }
    }
}
