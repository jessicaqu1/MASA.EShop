﻿using MASA.EShop.Contracts.Basket.Model.Web;
using MASA.EShop.Web.Client.Services.Basket;

namespace MASA.EShop.Web.Client.Pages.Basket;

[Authorize]
public partial class Basket : EShopPageBase
{
    private UserBasket _userBasket = new UserBasket("", new List<BasketItem>());

    class MyBreadcrumbItem : BreadcrumbItem
    {
        public string Icon { get; set; } = default!;
    }

    List<MyBreadcrumbItem> breadItems = new()
    {
        new MyBreadcrumbItem() { Text = "Cart", Disabled = false, Href = "/basket", Icon = "mdi-cart" },
        new MyBreadcrumbItem() { Text = "Address", Disabled = true, Href = "/basket/checkout", Icon = "mdi-domain" }
    };

    [Inject]
    private BasketService _baksetService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await LoadBasketAsync();
    }

    private async Task LoadBasketAsync()
    {
        try
        {
            //User.Identity.Name
            _userBasket = await _baksetService.GetBasketAsync("masa");
        }
        catch (Exception ex)
        {
            Message(ex.Message, AlertTypes.Error);
        }
    }

    private async Task RemoveItemAsync(int productId)
    {
        try
        {
            await _baksetService.RemoveItemAsync(User.Identity.Name, productId);
            await LoadBasketAsync();
        }
        catch (Exception ex)
        {
            Message(ex.Message, AlertTypes.Error);
        }
    }

    private void NavToCheckout()
    {
        if (_userBasket.Items.Any())
        {
            Navigation("basket/checkout");
            return;
        }
        Message("购物车中没有商品", AlertTypes.Warning);
    }
}

