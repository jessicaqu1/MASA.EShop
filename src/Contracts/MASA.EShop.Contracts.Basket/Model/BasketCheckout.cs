namespace MASA.EShop.Contracts.Basket.Model;

public record BasketCheckout(
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode,
    string CardNumber,
    string CardHolderName,
    DateTime CardExpiration,
    string CardSecurityNumber,
    int CardTypeId,
    string Buyer,
    Guid RequestId);

//public class BasketCheckout
//{
//    public string City { get; set; } = default!;

//    public string Street { get; set; } = default!;

//    public string State { get; set; } = default!;

//    public string Country { get; set; } = default!;

//    public string ZipCode { get; set; } = default!;

//    public string CardNumber { get; set; } = default!;

//    public string CardHolderName { get; set; } = default!;

//    public DateTime CardExpiration { get; set; }

//    public string CardSecurityNumber { get; set; } = default!;

//    public int CardTypeId { get; set; }

//    public string Buyer { get; set; } = default!;

//    public Guid RequestId { get; set; }
//}
