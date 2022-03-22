using FSH.WebApi.Application.Exchange.Traders;

namespace FSH.WebApi.Application.Exchange.Offers;

public class OfferDetailsDto : IDto
{
    public Guid Id { get; set; }
    public string CurrencyCode { get; set; } = default!;
    public decimal NetValue { get; set; }
    public decimal GrossValue { get; set; }
    public DeliveryCostType DeliveryCostType { get; set; }
    public decimal DeliveryCostGrossPrice { get; set; }
    public string? DeliveryCostDescription { get; set; }
    public string? Freebie { get; set; }
    public bool HasFreebies { get; set; }
    public bool HasReplacements { get; set; }
    public TraderDto Trader { get; set; } = default!;
    public IList<OfferProductDto> Products { get; set; } = default!;
}