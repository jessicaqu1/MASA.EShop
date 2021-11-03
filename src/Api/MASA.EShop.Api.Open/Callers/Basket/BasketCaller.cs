﻿namespace MASA.EShop.Api.Open.Callers.Basket;

public class BasketCaller : ServiceCaller
{
    private string getBasketUrl;
    private readonly string updateBasketUrl;
    private readonly string checkoutUrl;

    private readonly ILogger<BasketCaller> _logger;
    private readonly CatalogCaller _catalogCaller;

    public BasketCaller(
        IServiceProvider serviceProvider,
        IOptions<Settings> settings,
        ILogger<BasketCaller> logger,
        CatalogCaller catalogCaller) : base(serviceProvider)
    {
        _logger = logger;
        _catalogCaller = catalogCaller;
        BaseAddress = settings.Value.BasketUrl;

        var party = "/api/v1/basket/";
        getBasketUrl = $"{party}";
        updateBasketUrl = $"{party}updatebasket";
        checkoutUrl = $"{party}checkout";
    }

    public async Task RemoveItemAsync(string userId, int productId)
    {
        var currentBasket = await GetBasketAsync(userId);
        var basketItem = currentBasket.Items.FirstOrDefault(x => x.ProductId == productId);
        if (basketItem != null)
        {
            currentBasket.Items.Remove(basketItem);
            var response = await PostAsJsonAsync(updateBasketUrl, currentBasket);
        }
    }

    public async Task AddItemToBasketAsync(string userId, int productId)
    {
        var newItem = new
        {
            CatalogItemId = productId,
            BasketId = userId,
            Quantity = 1
        };

        // Step 1: Get the item from catalog
        var item = await _catalogCaller.GetCatalogById(newItem.CatalogItemId);
        // Step 2: Get current basket status
        var currentBasket = await GetBasketAsync(userId);
        // Step 3: Merge current status with new product
        var itemIndex = currentBasket.Items.FindIndex(a => a.ProductId == item.Id);
        if (itemIndex == -1)
        {
            currentBasket.Items.Add(new BasketItem(item.Id, item.Name, item.Price, newItem.Quantity, item.PictureFileName));
        }
        else
        {
            var basketItem = currentBasket.Items.ElementAt(itemIndex);
            currentBasket.Items[itemIndex] = basketItem with { Quantity = basketItem.Quantity + 1 };
        }

        // Step 4: Update basket
        var response = await PostAsJsonAsync(updateBasketUrl, currentBasket);
    }

    public async Task<UserBasket> GetBasketAsync(string userId)
    {
        return await GetFromJsonAsync<UserBasket>($"{getBasketUrl}{userId}");
    }

    public Task<UserBasket> UpdateBasketAsync(UserBasket basket)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CheckoutAsync(BasketCheckout basketCheckout)
    {
        var response = await PostAsJsonAsync(checkoutUrl, basketCheckout);

        try
        {
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError($"Basket Service Request CheckoutAsync Error:{e}");
        }
        return false;
    }
}

