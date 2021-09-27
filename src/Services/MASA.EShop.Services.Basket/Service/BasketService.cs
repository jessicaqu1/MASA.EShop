
namespace MASA.EShop.Services.Basket.Service;
public class BasketService : ServiceBase
{
    private const string DAPR_PUBSUB_NAME = "pubsub";
    private readonly IBasketRepository _repository;
    private readonly IIntegrationEventBus _eventBus = default!;

    public BasketService(
        IServiceCollection services,
        IBasketRepository repository,
        IIntegrationEventBus eventBus) : base(services)
    {
        _repository = repository;
        _eventBus = eventBus;

        App.MapGet("/api/v1/basket/{id}", GetBasketByIdAsync);
        App.MapPost("/api/v1/basket/updatebasket", UpdateBasketAsync);
        App.MapPost("/api/v1/basket/checkout", CheckoutAsync);
        App.MapDelete("/api/v1/basket/{id}", DeleteBasketByIdAsync);
    }

    public async Task<string> GetBasketByIdAsync(string id)
    {
        await _repository.GetBasketAsync(id);
        return $"GetBasketByIdAsync {id}";
    }

    public async Task<string> UpdateBasketAsync([FromBody] CustomerBasket value)
    {
        await _repository.UpdateBasketAsync(value);
        return $"UpdateBasketAsync {JsonSerializer.Serialize(value)}";
    }

    public async Task<string> CheckoutAsync([FromBody] BasketCheckout basketCheckout)
    {
        _eventBus.PublishAsync(new CheckoutAcceptedIntegrationEvent() { 
            //todo
        });
        return $"CheckoutAsync {JsonSerializer.Serialize(basketCheckout)}";
    }

    public async Task<string> DeleteBasketByIdAsync(string id)
    {
        await _repository.DeleteBasketAsync(id);
        return $"DeleteBasketByIdAsync {id}";
    }

    [Topic(DAPR_PUBSUB_NAME, "OrderStartedIntegrationEvent")]
    public async Task OrderStarted(OrderStartedIntegrationEvent @event)
    {
        /*var handler = _serviceProvider.GetRequiredService<OrderStatusChangedToSubmittedIntegrationEventHandler>();
        await handler.Handle(@event);*/

    }
}
