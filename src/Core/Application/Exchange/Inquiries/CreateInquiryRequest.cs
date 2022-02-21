using MassTransit;

namespace FSH.WebApi.Application.Exchange.Inquiries;

public class UserInquiriesSpec : Specification<Inquiry>, ISingleResultSpecification
{
    public UserInquiriesSpec(Guid userId) => Query.Where(i => i.CreatedBy == userId);
}

public class CreateInquiryRequest : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string Title { get; set; } = default!;
    public IList<Guid> RecipientIds { get; set; } = default!;
    public IList<InquiryProductDto> Products { get; set; } = default!;
}

public class InquiryProductValidator : CustomValidator<InquiryProductDto>
{
    public InquiryProductValidator()
    {
        RuleFor(p => p.Name).NotEmpty().MinimumLength(3).MaximumLength(100);
        RuleFor(p => p.Quantity).NotEmpty().GreaterThan(0);
        RuleFor(p => p.PreferredDeliveryDate).NotEmpty().GreaterThanOrEqualTo(DateTime.UtcNow);
    }
}

public class CreateInquiryRequestValidator : CustomValidator<CreateInquiryRequest>
{
    public CreateInquiryRequestValidator(IStringLocalizer<CreateInquiryRequestValidator> localizer)
    {
        RuleFor(i => i.Name).NotEmpty().MinimumLength(3).MaximumLength(60);
        RuleFor(i => i.Title).NotEmpty().MinimumLength(3).MaximumLength(100);

        RuleFor(i => i.RecipientIds)
            .Must(ids => ids.Count > 0)
            .WithMessage(localizer["inquiry.norecipients"]);

        RuleFor(i => i.Products)
            .Must(products => products.Count > 0)
            .WithMessage(localizer["inquiry.noproducts"]);

        RuleForEach(i => i.Products).SetValidator(new InquiryProductValidator());
    }
}

public class CreateInquiryRequestHandler : IRequestHandler<CreateInquiryRequest, Guid>
{
    private readonly ICurrentUser _currentUser;

    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Inquiry> _repository;

    public CreateInquiryRequestHandler(ICurrentUser currentUser, IRepositoryWithEvents<Inquiry> repository) =>
        (_currentUser, _repository) = (currentUser, repository);

    public async Task<Guid> Handle(CreateInquiryRequest request, CancellationToken cancellationToken)
    {
        Guid inquiryId = NewId.Next().ToGuid();
        List<InquiryProduct> products = new();

        foreach (InquiryProductDto product in request.Products)
        {
            products.Add(new InquiryProduct(inquiryId, product.Name, product.Quantity, product.PreferredDeliveryDate));
        }

        var spec = new UserInquiriesSpec(_currentUser.GetUserId());
        int referenceNumber = await _repository.CountAsync(spec, cancellationToken);
        var inquiry = new Inquiry(inquiryId, referenceNumber + 1, request.Name, request.Title, products, request.RecipientIds);

        await _repository.AddAsync(inquiry, cancellationToken);

        return inquiry.Id;
    }
}