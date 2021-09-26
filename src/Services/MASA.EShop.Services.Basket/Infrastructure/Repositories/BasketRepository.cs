
namespace MASA.EShop.Services.Basket.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly BasketDbContext _context;

        public BasketRepository(BasketDbContext context)
        { 
            _context = context; 
        }

        public async Task DeleteBasketAsync(string id)
        {
            var deleteItem = await _context.BasketItems.FindAsync(id);
            _context.BasketItems.Remove(deleteItem);
            await _context.SaveChangesAsync();
        }

        public async Task<CustomerBasket> GetBasketAsync(string customerId)
        {
            return await _context.CustomerBaskets.FirstOrDefaultAsync(a => a.BuyerId == customerId);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var updateItem = await GetBasketAsync(basket.BuyerId);
            updateItem.Items = basket.Items;
            await _context.SaveChangesAsync();
            return updateItem;
        }
    }
}
