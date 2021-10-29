using Dapr.Client;
using MASA.EShop.Api.Caller;
using System.Net.Http;

namespace MASA.EShop.Web.Client.Services
{
    public class OrderService : ServiceCaller
    {
        private readonly ILogger<OrderService> _logger;

        private readonly string getOrdersUrl = "";
        private readonly string cancelOrderUrl = "";
        private readonly string shipOrderUrl = "";

        private string party = "/api/v1/orders/";

        public override string BaseAddress { get; set; }


        public OrderService(HttpClient httpClient, DaprClient daprClient, IOptions<Settings> settings, ILogger<OrderService> logger) : base(httpClient, daprClient)
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

        public async Task ShipOrder(int orderNumber)
        {
            var order = new
            {
                OrderNumber = orderNumber
            };

            var stringContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("orderNumber",orderNumber.ToString())
            });

            var response = await PutAsync(shipOrderUrl, stringContent);

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new Exception("Error cancelling order, try later.");
            }

            response.EnsureSuccessStatusCode();
        }

        public async Task CancelOrder(int orderNumber)
        {
            var response = await PutAsync($"{cancelOrderUrl}/{orderNumber}", null);

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new Exception("Error cancelling order, try later.");
            }

            response.EnsureSuccessStatusCode();
        }
    }
}
