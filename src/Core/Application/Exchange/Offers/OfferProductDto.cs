namespace FSH.WebApi.Application.Exchange.Offers;

public class OfferProductDto : IDto
{
    public decimal? VatRate { get; set; }
    public int Quantity { get; set; }
    public decimal NetPrice { get; set; }
    public DateTime DeliveryDate { get; set; }
    public bool IsReplacement { get; set; }
    public string? ReplacementName { get; set; }
    public string? Freebie { get; set; }
    public Guid InquiryProductId { get; set; }
}

public class OfferProductValidator : CustomValidator<OfferProductDto>
{
    public OfferProductValidator()
    {
        RuleFor(p => p.VatRate).InclusiveBetween(0, 1);
        RuleFor(p => p.Quantity).NotEmpty().GreaterThan(0);
        RuleFor(p => p.NetPrice).NotEmpty().GreaterThan(0);
        RuleFor(p => p.DeliveryDate).NotEmpty().GreaterThanOrEqualTo(DateTime.UtcNow);
        RuleFor(p => p.IsReplacement).NotNull();
        RuleFor(p => p.ReplacementName).MinimumLength(3).MaximumLength(100);
        RuleFor(p => p.Freebie).MinimumLength(3).MaximumLength(2000);
        RuleFor(p => p.InquiryProductId).NotEmpty();
    }
}