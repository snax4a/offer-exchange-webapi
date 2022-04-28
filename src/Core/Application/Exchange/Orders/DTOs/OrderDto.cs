using FSH.WebApi.Application.Exchange.Traders.DTOs;

namespace FSH.WebApi.Application.Exchange.Orders.DTOs;

public class OrderDto : IDto
{
    public Guid Id { get; set; }
    public string CurrencyCode { get; set; } = default!;
    public decimal NetValue { get; set; }
    public decimal GrossValue { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedOn { get; set; }
    public TraderDto Trader { get; set; } = default!;
}