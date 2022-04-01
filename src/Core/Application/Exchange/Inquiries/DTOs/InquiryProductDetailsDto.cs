namespace FSH.WebApi.Application.Exchange.Inquiries.DTOs;

public class InquiryProductDetailsDto : IDto
{
    public Guid Id { get; set; }
    public Guid InquiryId { get; set; }
    public string Name { get; set; } = default!;
    public int Quantity { get; set; }
    public DateOnly PreferredDeliveryDate { get; set; }
}

public class InquiryProductDetailsValidator : CustomValidator<InquiryProductDetailsDto>
{
    public InquiryProductDetailsValidator()
    {
        RuleFor(p => p.Id).NotEmpty().Must(id => id != Guid.Empty);
        RuleFor(p => p.InquiryId).NotEmpty().Must(id => id != Guid.Empty);
        RuleFor(p => p.Name).NotEmpty().MinimumLength(3).MaximumLength(100);
        RuleFor(p => p.Quantity).NotEmpty().GreaterThan(0);
        RuleFor(p => p.PreferredDeliveryDate).NotEmpty();
    }
}