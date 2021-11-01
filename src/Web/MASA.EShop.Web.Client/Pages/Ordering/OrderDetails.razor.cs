using MASA.EShop.Contracts.Ordering.Model;
using MASA.EShop.Web.Client.Services.Ordering;

namespace MASA.EShop.Web.Client.Pages.Ordering;

[Authorize]
public partial class OrderDetails : EShopPageBase
{
    private bool _loading = false;
    private Order _order = new Order(0, DateTime.MinValue, "", "", "", "", "", "", new List<OrderItem>() {
                new OrderItem(0,"",0,0,"")
            });

    [Parameter]
    public int OrderNumber { get; set; }

    [Inject]
    private OrderService _orderService { get; set; } = default!;

    protected async override Task OnInitializedAsync()
    {
        try
        {
            await base.OnInitializedAsync();
            if (IsAuthenticated)
            {
                _order = await _orderService.GetOrder(User.Identity.Name, OrderNumber);
            }
        }
        catch (Exception ex)
        {
            Message(ex.Message, AlertTypes.Error);
        }
    }
}

