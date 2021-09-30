namespace MASA.EShop.Services.Ordering.Actors
{
    public class OrderingProcessActor : Actor, IOrderingProcessActor, IRemindable
    {
        private const string OrderDetailsStateName = "OrderDetails";
        private const string OrderStatusStateName = "OrderStatus";

        private readonly IEventBus _eventBus;
        private readonly ILogger<OrderingProcessActor> _logger;

        private Guid OrderId => Guid.Parse(Id.GetId());

        public OrderingProcessActor(
            ActorHost host,
            IEventBus eventBus,
            ILogger<OrderingProcessActor> logger) : base(host)
        {
            _logger = logger;
            _eventBus = eventBus;
        }

        public async Task<bool> Cancel()
        {
            var orderStatus = await StateManager.TryGetStateAsync<OrderStatus>(OrderStatusStateName);
            if (!orderStatus.HasValue)
            {
                _logger.LogWarning("Order with Id: {OrderId} cannot be cancelled because it doesn't exist",
                    OrderId);

                return false;
            }

            if (orderStatus.Value.Id == OrderStatus.Paid.Id || orderStatus.Value.Id == OrderStatus.Shipped.Id)
            {
                _logger.LogWarning("Order with Id: {OrderId} cannot be cancelled because it's in status {Status}",
                    OrderId, orderStatus.Value.Name);

                return false;
            }

            await StateManager.SetStateAsync(OrderStatusStateName, OrderStatus.Cancelled);

            var order = await StateManager.GetStateAsync<Order>(OrderDetailsStateName);

            await _eventBus.PublishAsync(new OrderStatusChangedToCancelledIntegrationEvent(
                OrderId,
                OrderStatus.Cancelled.Name,
                $"The order was cancelled by buyer.",
                order.UserName));

            return true;
        }

        public async Task<bool> Ship()
        {
            var statusChanged = await TryUpdateOrderStatusAsync(OrderStatus.Paid, OrderStatus.Shipped);
            if (statusChanged)
            {
                var order = await StateManager.GetStateAsync<Order>(OrderDetailsStateName);

                await _eventBus.PublishAsync(new OrderStatusChangedToShippedIntegrationEvent(
                    OrderId,
                    OrderStatus.Shipped.Name,
                    "The order was shipped.",
                    order.UserName));

                return true;
            }

            return false;
        }

        private async Task<bool> TryUpdateOrderStatusAsync(OrderStatus expectedOrderStatus, OrderStatus newOrderStatus)
        {
            var orderStatus = await StateManager.TryGetStateAsync<OrderStatus>(OrderStatusStateName);
            if (!orderStatus.HasValue)
            {
                _logger.LogWarning("Order with Id: {OrderId} cannot be updated because it doesn't exist",
                    OrderId);

                return false;
            }

            if (orderStatus.Value.Id != expectedOrderStatus.Id)
            {
                _logger.LogWarning("Order with Id: {OrderId} is in status {Status} instead of expected status {ExpectedStatus}",
                    OrderId, orderStatus.Value.Name, expectedOrderStatus.Name);

                return false;
            }

            await StateManager.SetStateAsync(OrderStatusStateName, newOrderStatus);

            return true;
        }

        public Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
        {
            throw new NotImplementedException();
        }

        public async Task NotifyPaymentSucceeded()
        {
            var statusChanged = await TryUpdateOrderStatusAsync(OrderStatus.Validated, OrderStatus.Paid);
            if (statusChanged)
            {
                var order = await StateManager.GetStateAsync<Order>(OrderDetailsStateName);
                await _eventBus.PublishAsync(new OrderStatusChangedToPaidIntegrationEvent(
                    OrderId,
                    OrderStatus.Paid.Name,
                    "The payment was performed at a simulated \"American Bank checking bank account ending on XX35071\"",
                    order.UserName,
                    order.OrderItems.Select(orderItem => new OrderStockItem(orderItem.ProductId, orderItem.Units))));

                // Simulate a work time by setting a reminder.
                //await RegisterReminderAsync(
                //    PaymentSucceededReminder,
                //    null,
                //    TimeSpan.FromSeconds(10),
                //    TimeSpan.FromMilliseconds(-1));
            }
        }

        public async Task NotifyPaymentFailed()
        {
            var statusChanged = await TryUpdateOrderStatusAsync(OrderStatus.Validated, OrderStatus.Paid);
            if (statusChanged)
            {
                var order = await StateManager.GetStateAsync<Order>(OrderDetailsStateName);

                await _eventBus.PublishAsync(new OrderStatusChangedToCancelledIntegrationEvent(
                    OrderId,
                    OrderStatus.Cancelled.Name,
                    "The order was cancelled because payment failed.",
                    order.UserName));

                // Simulate a work time by setting a reminder.
                //await RegisterReminderAsync(
                //    PaymentFailedReminder,
                //    null,
                //    TimeSpan.FromSeconds(10),
                //    TimeSpan.FromMilliseconds(-1));
            }
        }
    }
}
