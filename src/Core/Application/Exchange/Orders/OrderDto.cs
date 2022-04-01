using FSH.WebApi.Application.Exchange.Traders.DTOs;

namespace FSH.WebApi.Application.Exchange.Orders;

public class OrderDto : IDto
{
    public Guid Id { get; set; }
    public string CurrencyCode { get; set; } = default!;
    public decimal NetValue { get; set; }
    public decimal GrossValue { get; set; }
    public DeliveryCostType DeliveryCostType { get; set; }
    public decimal DeliveryCostGrossPrice { get; set; }
    public string? DeliveryCostDescription { get; set; }
    public TraderDto Trader { get; set; } = default!;
}