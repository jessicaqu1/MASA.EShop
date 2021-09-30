namespace MASA.EShop.Services.Ordering.Service
{
    public class OrderEventService : ServiceBase
    {
        private const string DaprPubSubName = "pubsub";

        public OrderEventService(IServiceCollection services) : base(services)
        {
            App.MapPost("/api/v1/orders/paymentsucceeded", OrderPaymentSucceeded);
            App.MapPost("/api/v1/orders/paymentfailed", OrderPaymentFailed);
            App.MapPost("/api/v1/orders/StatusChangedToPaid", OrderStatusChangedToPaid);
            App.MapPost("/api/v1/orders/StatusChangedToShipped", OrderStatusChangedToShipped);
            App.MapPost("/api/v1/orders/StatusChangedToCancelled", OrderStatusChangedToCancelled);
        }

        //[HttpPost("UserCheckoutAccepted")]
        //[Topic(DaprPubSubName, "UserCheckoutAcceptedIntegrationEvent")]
        //public async Task Handle(UserCheckoutAcceptedIntegrationEvent integrationEvent)
        //{
        //    if (integrationEvent.RequestId != Guid.Empty)
        //    {
        //        var orderingProcess = GetOrderingProcessActor(integrationEvent.RequestId);

        //        await orderingProcess.Submit(integrationEvent.UserId, integrationEvent.UserName,
        //            integrationEvent.Street, integrationEvent.City, integrationEvent.ZipCode,
        //            integrationEvent.State, integrationEvent.Country, integrationEvent.Basket);
        //    }
        //    else
        //    {
        //        _logger.LogWarning("Invalid IntegrationEvent - RequestId is missing - {@IntegrationEvent}", integrationEvent);
        //    }
        //}
        private static IOrderingProcessActor GetOrderingProcessActor(Guid orderId)
        {
            var actorId = new ActorId(orderId.ToString());
            return ActorProxy.Create<IOrderingProcessActor>(actorId, nameof(OrderingProcessActor));
        }

        [Topic(DaprPubSubName, "OrderPaymentSucceededIntegrationEvent")]
        public Task OrderPaymentSucceeded(OrderPaymentSucceededIntegrationEvent integrationEvent)
        {
            return GetOrderingProcessActor(integrationEvent.OrderId).NotifyPaymentSucceeded();
        }

        [Topic(DaprPubSubName, "OrderPaymentFailedIntegrationEvent")]
        public Task OrderPaymentFailed(OrderPaymentFailedIntegrationEvent integrationEvent)
        {
            return GetOrderingProcessActor(integrationEvent.OrderId).NotifyPaymentFailed();
        }

        [HttpPost("OrderStatusChangedToPaid")]
        [Topic(DaprPubSubName, "OrderStatusChangedToPaidIntegrationEvent")]
        public Task OrderStatusChangedToPaid(OrderStatusChangedToPaidIntegrationEvent integrationEvent)
        {
            // Save the updated status in the read model and notify the client via SignalR.
            //return UpdateReadModelAndSendNotificationAsync(integrationEvent.OrderId,
            //    integrationEvent.OrderStatus, integrationEvent.Description, integrationEvent.BuyerName);
        }

        [HttpPost("OrderStatusChangedToShipped")]
        [Topic(DaprPubSubName, "OrderStatusChangedToShippedIntegrationEvent")]
        public Task OrderStatusChangedToShipped(OrderStatusChangedToShippedIntegrationEvent integrationEvent)
        {
            // Save the updated status in the read model and notify the client via SignalR.
            //return UpdateReadModelAndSendNotificationAsync(integrationEvent.OrderId,
            //    integrationEvent.OrderStatus, integrationEvent.Description, integrationEvent.BuyerName);
        }

        [HttpPost("OrderStatusChangedToCancelled")]
        [Topic(DaprPubSubName, "OrderStatusChangedToCancelledIntegrationEvent")]
        public Task OrderStatusChangedToCancelled(OrderStatusChangedToCancelledIntegrationEvent integrationEvent)
        {
            // Save the updated status in the read model and notify the client via SignalR.
            //return UpdateReadModelAndSendNotificationAsync(integrationEvent.OrderId,
            //    integrationEvent.OrderStatus, integrationEvent.Description, integrationEvent.BuyerName);
        }
    }
}
