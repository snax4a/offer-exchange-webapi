namespace FSH.WebApi.Application.Exchange.Offers;

public class DeliveryCostDto : IDto
{
    public DeliveryCostType Type { get; set; }
    public decimal? GrossPrice { get; set; }
    public string? Description { get; set; }
}

public class DeliveryCostValidator : CustomValidator<DeliveryCostDto>
{
    public DeliveryCostValidator()
    {
        RuleFor(dc => dc.Type).NotEmpty();

        RuleFor(dc => dc.GrossPrice)
            .NotEmpty()
            .GreaterThan(0)
            .When(dc => dc.Type == DeliveryCostType.Fixed);

        RuleFor(dc => dc.Description)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(2000)
            .When(dc => dc.Type == DeliveryCostType.Variable);
    }
}