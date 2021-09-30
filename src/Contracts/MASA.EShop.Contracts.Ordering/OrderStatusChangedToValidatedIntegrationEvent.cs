using MASA.Contrib.Dispatcher.IntegrationEvents.Dapr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.EShop.Contracts.Ordering
{
    public class OrderStatusChangedToValidatedIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; set; }

        public string OrderStatus { get; set; }

        public string Description { get; set; }

        public string BuyerName { get; set; }

        public decimal Total { get; set; }

        public override string Topic { get ; set ; } = nameof(OrderStatusChangedToValidatedIntegrationEvent);

        public OrderStatusChangedToValidatedIntegrationEvent()
        {
        }

        public OrderStatusChangedToValidatedIntegrationEvent(Guid orderId, string orderStatus,
            string description, string buyerName, decimal total)
        {
            OrderId = orderId;
            OrderStatus = orderStatus;
            Description = description;
            BuyerName = buyerName;
            Total = total;
        }
    }
}
