using MASA.Contrib.Dispatcher.IntegrationEvents.Dapr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.EShop.Contracts.Ordering
{
    public class OrderStatusChangedToPaidIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; set; }
        public string OrderStatus { get; set; }
        public string Description { get; set; }
        public string BuyerName { get; set; }
        public IEnumerable<OrderStockItem> OrderStockItems { get; set; }
        public override string Topic { get; set; } = nameof(OrderStatusChangedToPaidIntegrationEvent);

        public OrderStatusChangedToPaidIntegrationEvent()
        {
        }

        public OrderStatusChangedToPaidIntegrationEvent(Guid orderId, string orderStatus,
            string description, string buyerName, IEnumerable<OrderStockItem> orderStockItems)
        {
            OrderId = orderId;
            Description = description;
            OrderStatus = orderStatus;
            BuyerName = buyerName;
            OrderStockItems = orderStockItems;
        }
    }

    public class OrderStockItem
    {
        public int ProductId { get; set; }
        public int Units { get; set; }

        public OrderStockItem(int productId, int units)
        {
            ProductId = productId;
            Units = units;
        }
    }
}
