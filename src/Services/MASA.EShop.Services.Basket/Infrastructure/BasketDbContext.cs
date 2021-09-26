using MASA.EShop.Services.Basket.Entities;
using Microsoft.EntityFrameworkCore;

namespace MASA.EShop.Services.Basket.Infrastructure
{
    public class BasketDbContext : DbContext
    {
        public BasketDbContext(DbContextOptions<BasketDbContext> options) 
            : base(options)
        {

        }

        public DbSet<BasketItem> BasketItems { get; set; } = null!;

        public DbSet<CustomerBasket> CustomerBaskets { get; set; } = null!;
    }
}
