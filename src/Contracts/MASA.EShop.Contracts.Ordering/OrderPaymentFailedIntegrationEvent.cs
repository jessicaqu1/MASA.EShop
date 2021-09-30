using MASA.Contrib.Dispatcher.IntegrationEvents.Dapr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.EShop.Contracts.Ordering
{
    public class OrderPaymentFailedIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; set; }
        public override string Topic { get ; set ; } = nameof(OrderPaymentFailedIntegrationEvent);

        public OrderPaymentFailedIntegrationEvent()
        {
        }

        public OrderPaymentFailedIntegrationEvent(Guid orderId) => OrderId = orderId;
    }
}
