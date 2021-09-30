using MASA.EShop.Contracts.Basket.Model;

namespace MASA.EShop.Services.Basket.Infrastructure.Repositories;

public interface IBasketRepository
{
    Task<CustomerBasket> GetBasketAsync(string customerId);
    Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
    Task DeleteBasketAsync(string id);
}