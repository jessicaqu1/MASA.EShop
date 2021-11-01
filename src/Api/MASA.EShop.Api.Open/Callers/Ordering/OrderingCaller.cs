using MASA.EShop.Contracts.Ordering.Model;
using Microsoft.Extensions.Options;

namespace MASA.EShop.Api.Open.Callers.Ordering
{
    public class OrderingCaller : ServiceCaller
    {
        private readonly ILogger<OrderingCaller> _logger;

        private readonly string getOrdersUrl = "";
        private readonly string cancelOrderUrl = "";
        private readonly string shipOrderUrl = "";

        private string party = "/api/v1/orders/";

        public OrderingCaller(
            IServiceProvider serviceProvider,
            IOptions<Settings> settings,
            ILogger<OrderingCaller> logger) : base(serviceProvider)
        {
            BaseAddress = settings.Value.OrderingUrl;
            _logger = logger;

            getOrdersUrl = $"{party}list";
            cancelOrderUrl = $"{party}cancel";
            shipOrderUrl = $"{party}ship";
        }

        public async Task<List<OrderSummary>> GetMyOrders(string userId)
        {
            return await GetFromJsonAsync<List<OrderSummary>>($"{getOrdersUrl}?userId={userId}") ?? new List<OrderSummary>();
        }

        public async Task<Order> GetOrder(string userId, int orderNumber)
        {
            return await GetFromJsonAsync<Order>($"{party}{userId}/{orderNumber}");
        }

        public async Task<bool> ShipOrder(int orderNumber)
        {
            var stringContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("orderNumber",orderNumber.ToString())
            });

            var response = await PutAsync(shipOrderUrl, stringContent);
            var result = true;
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                _logger.LogError($"Ordering Service Request ShipOrder Error:{e}");
                result = false;
            }
            return result;
        }

        public async Task<bool> CancelOrder(int orderNumber)
        {
            var response = await PutAsync($"{cancelOrderUrl}/{orderNumber}", null);
            var result = true;
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                _logger.LogError($"Ordering Service Request CancelOrder Error:{e}");
                result = false;
            }
            return result;
        }
    }
}
