using FSH.WebApi.Application.Exchange.Inquiries.DTOs;
using FSH.WebApi.Application.Exchange.Inquiries.Specifications;
using MassTransit;

namespace FSH.WebApi.Application.Exchange.Inquiries;

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
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(p => p.Name).NotEmpty().MinimumLength(3).MaximumLength(100);
        RuleFor(p => p.Quantity).NotEmpty().GreaterThan(0);
        RuleFor(p => p.PreferredDeliveryDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .Unless(p => p.PreferredDeliveryDate is null);
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
    private readonly IJobService _jobService;

    public CreateInquiryRequestHandler(ICurrentUser currentUser, IRepositoryWithEvents<Inquiry> repository, IJobService jobService)
    {
        (_currentUser, _repository, _jobService) = (currentUser, repository, jobService);
    }

    public async Task<Guid> Handle(CreateInquiryRequest request, CancellationToken ct)
    {
        Guid inquiryId = NewId.Next().ToGuid();
        List<InquiryProduct> products = new();

        foreach (InquiryProductDto product in request.Products)
        {
            products.Add(new InquiryProduct(inquiryId, product.Name, product.Quantity, product.PreferredDeliveryDate));
        }

        var spec = new UserInquiriesSpec(_currentUser.GetUserId());
        int referenceNumber = await _repository.CountAsync(spec, ct);
        var inquiry = new Inquiry(inquiryId, referenceNumber + 1, request.Name, request.Title, products, request.RecipientIds);

        await _repository.AddAsync(inquiry, ct);

        foreach (Guid traderId in request.RecipientIds)
        {
            _jobService.Enqueue<IInquirySenderJob>(x => x.SendAsync(inquiry.Id, traderId, CancellationToken.None));
        }

        return inquiry.Id;
    }
}