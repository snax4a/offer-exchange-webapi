using FSH.WebApi.Application.Exchange.Inquiries.DTOs;

namespace FSH.WebApi.Application.Exchange.Offers;

public class OfferProductDto : IDto
{
    public string CurrencyCode { get; set; } = default!;
    public short? VatRate { get; set; }
    public int Quantity { get; set; }
    public long NetPrice { get; set; }
    public long NetValue { get; set; }
    public long GrossValue { get; set; }
    public DateOnly DeliveryDate { get; set; }
    public bool IsReplacement { get; set; }
    public string? ReplacementName { get; set; }
    public string? Freebie { get; set; }
    public InquiryProductDetailsDto InquiryProduct { get; set; } = default!;
}

public class OfferProductValidator : CustomValidator<OfferProductDto>
{
    public OfferProductValidator()
    {
        RuleFor(p => p.CurrencyCode).NotEmpty().Length(3);
        RuleFor(p => p.VatRate).InclusiveBetween((short)0, (short)100);
        RuleFor(p => p.Quantity).NotEmpty().GreaterThan(0);
        RuleFor(p => p.NetPrice).NotEmpty().GreaterThan(0L);
        RuleFor(p => p.DeliveryDate).NotEmpty().GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow));
        RuleFor(p => p.IsReplacement).NotNull();
        RuleFor(p => p.ReplacementName).MinimumLength(3).MaximumLength(100);
        RuleFor(p => p.Freebie).MinimumLength(3).MaximumLength(2000);
        RuleFor(p => p.InquiryProduct).SetValidator(new InquiryProductDetailsValidator());
    }
}