namespace MASA.EShop.Services.Basket.Infrastructure.Repositories;

public class BasketRepository : IBasketRepository
{
    private const string StoreName = "eshop-statestore";

    private readonly DaprClient _dapr;

    public BasketRepository(DaprClient dapr)
    {
        _dapr = dapr;
    }

    public async Task DeleteBasketAsync(string id)
    {
        //var deleteItem = await _context.BasketItems.FindAsync(id);
        //_context.BasketItems.Remove(deleteItem);
        //await _context.SaveChangesAsync();
        await _dapr.DeleteStateAsync(StoreName, id);
    }

    public async Task<CustomerBasket> GetBasketAsync(string customerId)
    {
        return await _dapr.GetStateAsync<CustomerBasket>(StoreName, customerId);
        //return await _context.CustomerBaskets.FirstOrDefaultAsync(a => a.BuyerId == customerId);
    }

    public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
    {
        var state = await _dapr.GetStateEntryAsync<CustomerBasket>(StoreName, basket.BuyerId);
        state.Value = basket;
        await state.SaveAsync();

        return await GetBasketAsync(basket.BuyerId);
    }

    //public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
    //{
    //    var updateItem = await GetBasketAsync(basket.BuyerId);
    //    updateItem.Items = basket.Items;
    //    await _context.SaveChangesAsync();
    //    return updateItem;
    //}
}
