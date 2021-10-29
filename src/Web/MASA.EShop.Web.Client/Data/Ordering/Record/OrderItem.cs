﻿namespace MASA.EShop.Web.Client.Data.Ordering.Record;

public record OrderItem(
        int ProductId,
        string ProductName,
        decimal UnitPrice,
        int Units,
        string PictureFileName)
{
    public decimal Total => UnitPrice * Units;

    public string GetFormattedUnitPrice() => UnitPrice.ToString("0.00");

    public string GetFormattedTotal() => Total.ToString("0.00");

    public string GetPictureUrl() => $"./img/{PictureFileName}";
}

