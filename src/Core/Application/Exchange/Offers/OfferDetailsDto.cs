using FSH.WebApi.Application.Exchange.Inquiries.DTOs;
using FSH.WebApi.Application.Exchange.Traders;

namespace FSH.WebApi.Application.Exchange.Offers;

public class OfferDetailsDto : IDto
{
    public Guid Id { get; set; }
    public string CurrencyCode { get; set; } = default!;
    public long NetValue { get; set; }
    public long GrossValue { get; set; }
    public DeliveryCostType DeliveryCostType { get; set; }
    public long DeliveryCostGrossPrice { get; set; }
    public string? DeliveryCostDescription { get; set; }
    public DateOnly? ExpirationDate { get; set; }
    public string? Freebie { get; set; }
    public bool HasFreebies { get; set; }
    public bool HasReplacements { get; set; }
    public TraderDto Trader { get; set; } = default!;
    public InquiryDto Inquiry { get; set; } = default!;
    public IList<OfferProductDto> Products { get; set; } = default!;
}