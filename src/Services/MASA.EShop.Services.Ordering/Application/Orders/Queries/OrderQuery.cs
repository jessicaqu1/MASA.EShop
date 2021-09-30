namespace MASA.EShop.Services.Ordering.Application.Orders.Queries
{
    public class OrderQuery : Query<Order?>
    {
        public int OrderNumber { get; set; }

        public override Order? Result { get; set; }
    }
}
