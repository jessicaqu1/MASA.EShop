namespace MASA.EShop.Services.Ordering.Service
{
    public class OrderingService : ServiceBase
    {
        private readonly IEventBus _eventBus = default!;

        public OrderingService(IServiceCollection services, IEventBus eventBus) : base(services)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

            App.MapPut("/api/v1/orders/cancel", CancelOrderAsync);
            App.MapPut("/api/v1/orders/ship", ShipOrderAsync);
            App.MapGet("/api/v1/orders/{orderNumber:int}", ShipOrderAsync);
            App.MapGet("/api/v1/orders/list", GetOrdersAsync);
            App.MapGet("/api/v1/orders/cardtypes", GetCardTypesAsync);
        }

        public async Task<IResult> CancelOrderAsync(int orderNumber, [FromHeader(Name = "x-requestid")] string requestId)
        {
            try
            {
                if (!Guid.TryParse(requestId, out Guid guid) || guid == Guid.Empty)
                {
                    throw new Exception("invalid requestid");
                }
                var orderCanelCommand = new OrderCancelCommand
                {
                    OrderNumber = orderNumber
                };
                await _eventBus.PublishAsync(orderCanelCommand);
                return Results.Ok();
            }
            catch
            {
                return Results.BadRequest();
            }
        }

        public async Task<IResult> ShipOrderAsync(int orderNumber, [FromHeader(Name = "x-requestid")] string requestId)
        {
            try
            {
                if (!Guid.TryParse(requestId, out Guid guid) || guid == Guid.Empty)
                {
                    throw new Exception("invalid requestid");
                }
                var orderShipCommand = new OrderShipCommand
                {
                    OrderNumber = orderNumber
                };
                await _eventBus.PublishAsync(orderShipCommand);
                return Results.Ok();
            }
            catch
            {
                return Results.BadRequest();
            }
        }

        public async Task<IResult> GetOrderAsync(int orderNumber)
        {
            var orderQuery = new OrderQuery
            {
                OrderNumber = orderNumber
            };
            await _eventBus.PublishAsync(orderQuery);
            if (orderQuery.Result is null)
            {
                return Results.NotFound();
            }
            else
            {
                return Results.Ok(OrderDto.FromOrder(orderQuery.Result));
            }
        }

        public async Task<IResult> GetOrdersAsync()
        {
            var ordersQuery = new OrdersQuery
            {
                ByuerId = ""
            };
            await _eventBus.PublishAsync(ordersQuery);
            return Results.Ok(ordersQuery.Result.Select(OrderSummaryDto.FromOrderSummary));
        }

        public async Task<IResult> GetCardTypesAsync()
        {
            var cardTypesQuery = new CardTypesQuery();
            await _eventBus.PublishAsync(cardTypesQuery);
            return Results.Ok(cardTypesQuery.Result.Select(CardTypeDto.FromCardType));
        }
    }
}
