using MASA.Contrib.Dispatcher.IntegrationEvents.Dapr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.EShop.Contracts.Ordering
{
    public class OrderPaymentSucceededIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; set; }
        public override string Topic { get ; set ; } = nameof(OrderPaymentSucceededIntegrationEvent);

        public OrderPaymentSucceededIntegrationEvent()
        {
        }

        public OrderPaymentSucceededIntegrationEvent(Guid orderId) => OrderId = orderId;
    }
}
