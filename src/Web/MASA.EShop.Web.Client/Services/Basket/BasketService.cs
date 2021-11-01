using MASA.EShop.Contracts.Basket.Model;
using MASA.EShop.Contracts.Basket.Model.Web;

namespace MASA.EShop.Web.Client.Services.Basket
{
    public class BasketService : ServiceCaller
    {
        private readonly IServiceProvider _serviceProvider;

        private string getBasketUrl;
        private readonly string addItemUrl;
        private readonly string checkoutUrl;
        private readonly string removeItemUrl;

        public BasketService(
            IServiceProvider serviceProvider,
            IOptions<Settings> settings) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            BaseAddress = settings.Value.ApiGatewayUrlExternal;

            var party = "/api/v1/basket/";
            getBasketUrl = $"{party}";
            addItemUrl = $"{party}additem";
            removeItemUrl = $"{party}removeitem";
            checkoutUrl = $"{party}checkout";
        }

        protected override HttpMessageHandler? CreateHttpMessageHandler()
        {
            return _serviceProvider.GetService<HttpClientAuthorizationDelegatingHandler>();
        }

        public async Task RemoveItemAsync(string userId, int productId)
        {
            await PutAsync($"{removeItemUrl}/{userId}/{productId}", null);
        }

        public async Task AddItemToBasketAsync(string userId, int productId)
        {
            await PutAsync($"{addItemUrl}/{userId}/{productId}", null);
        }

        public async Task<UserBasket> GetBasketAsync(string userId)
        {
            return await GetFromJsonAsync<UserBasket>($"{getBasketUrl}{userId}");
        }

        public Task<UserBasket> UpdateBasketAsync(UserBasket basket)
        {
            throw new NotImplementedException();
        }

        public async Task CheckoutAsync(BasketCheckout basketCheckout)
        {
            var response = await PostAsJsonAsync(checkoutUrl, basketCheckout);

            response.EnsureSuccessStatusCode();
        }
    }
}
