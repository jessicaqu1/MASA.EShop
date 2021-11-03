namespace MASA.EShop.Web.Client.Services.Ordering;

public class OrderService : ServiceCaller
{
    private readonly IServiceProvider _serviceProvider;

    private readonly string getOrdersUrl = "";
    private readonly string cancelOrderUrl = "";
    private readonly string shipOrderUrl = "";

    private string party = "/api/v1/orders/";

    public OrderService(IServiceProvider serviceProvider,
                        IOptions<Settings> settings) : base(serviceProvider)
    {
        _serviceProvider = serviceProvider;
        BaseAddress = settings.Value.ApiGatewayUrlExternal;

        getOrdersUrl = $"{party}list";
        cancelOrderUrl = $"{party}cancel";
        shipOrderUrl = $"{party}ship";
    }

    protected override HttpMessageHandler? CreateHttpMessageHandler()
    {
        return _serviceProvider.GetService<HttpClientAuthorizationDelegatingHandler>();
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
        response.EnsureSuccessStatusCode();
    }

    public async Task CancelOrder(int orderNumber)
    {
        var response = await PutAsync($"{cancelOrderUrl}/{orderNumber}", null);
        response.EnsureSuccessStatusCode();
    }
}

