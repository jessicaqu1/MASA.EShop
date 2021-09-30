using MASA.Contrib.Dispatcher.IntegrationEvents.Dapr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.EShop.Contracts.Ordering
{
    public class OrderStatusChangedToCancelledIntegrationEvent: IntegrationEvent
    {
        public Guid OrderId { get; set; }
        public string OrderStatus { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string BuyerName { get; set; } = default!;
        public override string Topic { get ; set ; } = nameof(OrderStatusChangedToCancelledIntegrationEvent);

        public OrderStatusChangedToCancelledIntegrationEvent()
        {
        }

        public OrderStatusChangedToCancelledIntegrationEvent(Guid orderId, string orderStatus,
            string description, string buyerName)
        {
            OrderId = orderId;
            OrderStatus = orderStatus;
            Description = description;
            BuyerName = buyerName;
        }
    }
}
