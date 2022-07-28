using FSH.Webapi.Core.Application.FeatureUsage;
using FSH.WebApi.Application.Exchange.Addresses.Specifications;
using FSH.WebApi.Application.Exchange.Inquiries.DTOs;
using FSH.WebApi.Application.Exchange.Inquiries.Specifications;
using FSH.WebApi.Core.Shared.FeatureUsage;
using MassTransit;

namespace FSH.WebApi.Application.Exchange.Inquiries;

public class CreateInquiryRequest : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string Title { get; set; } = default!;
    public Guid UserAddressId { get; set; }
    public IList<Guid> RecipientIds { get; set; } = default!;
    public IList<InquiryProductDto> Products { get; set; } = default!;
}

public class InquiryProductValidator : CustomValidator<InquiryProductDto>
{
    public InquiryProductValidator()
    {
        CascadeMode = CascadeMode.Stop;

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
        CascadeMode = CascadeMode.Stop;

        RuleFor(i => i.Name).NotEmpty().MinimumLength(3).MaximumLength(60);
        RuleFor(i => i.Title).NotEmpty().MinimumLength(3).MaximumLength(100);
        RuleFor(i => i.UserAddressId).NotEmpty();

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
    private readonly IReadRepository<UserAddress> _addressRepo;
    private readonly IJobService _jobService;
    private readonly IFeatureUsageService _featureUsageService;
    private readonly IStringLocalizer<CreateInquiryRequestHandler> _localizer;

    public CreateInquiryRequestHandler(
        ICurrentUser currentUser,
        IRepositoryWithEvents<Inquiry> repository,
        IReadRepository<UserAddress> addressRepo,
        IJobService jobService,
        IFeatureUsageService featureUsageService,
        IStringLocalizer<CreateInquiryRequestHandler> localizer)
    {
        _currentUser = currentUser;
        _repository = repository;
        _addressRepo = addressRepo;
        _jobService = jobService;
        _featureUsageService = featureUsageService;
        _localizer = localizer;
    }

    public async Task<Guid> Handle(CreateInquiryRequest req, CancellationToken ct)
    {
        short numberOfProducts = (short)req.Products.Count;
        short numberOfRecipients = (short)req.RecipientIds.Count;
        await CheckUsageLimits(numberOfRecipients, numberOfProducts);
        var inquiry = await CreateInquiry(req, ct);
        await _featureUsageService.IncrementUsage(AppFeatureIds.Inquiries_MonthlyCount);
        return inquiry.Id;
    }

    // Check if the user has not exceeded his plan limits.
    // TODO: In future we should abstract this to a middleware.
    private async Task CheckUsageLimits(short numberOfRecipients, short numberOfProducts)
    {
        if (!await _featureUsageService.CanUseFeature(AppFeatureIds.Inquiries_MonthlyCount))
            throw new PaymentRequiredException(_localizer["inquiry.sendlimitreached"]);

        if (!await _featureUsageService.CanUseFeature(AppFeatureIds.Inquiries_NumberOfRecipients, numberOfRecipients))
            throw new PaymentRequiredException(_localizer["inquiry.recipientlimitreached"]);

        if (!await _featureUsageService.CanUseFeature(AppFeatureIds.Inquiries_NumberOfProducts, numberOfProducts))
            throw new PaymentRequiredException(_localizer["inquiry.productlimitreached"]);
    }

    private async Task<Inquiry> CreateInquiry(CreateInquiryRequest req, CancellationToken ct)
    {
        var userId = _currentUser.GetUserId();
        var addressSpec = new UserAddressByIdSpec(req.UserAddressId, userId);
        var userAddress = await _addressRepo.GetBySpecAsync(addressSpec, ct);
        if (userAddress is null) throw new ConflictException(_localizer["address.notfound"]);

        Guid inquiryId = NewId.Next().ToGuid();
        List<InquiryProduct> products = new();

        foreach (InquiryProductDto product in req.Products)
        {
            products.Add(new InquiryProduct(inquiryId, product.Name, product.Quantity, product.PreferredDeliveryDate));
        }

        var inquirySpec = new UserInquiriesSpec(userId);
        int referenceNumber = await _repository.CountAsync(inquirySpec, ct);
        var inquiry = new Inquiry(inquiryId, referenceNumber + 1, req.Name, req.Title, userAddress.Address, products, req.RecipientIds);

        await _repository.AddAsync(inquiry, ct);

        foreach (Guid traderId in req.RecipientIds)
        {
            _jobService.Enqueue<IInquirySenderJob>(x => x.SendAsync(inquiry.Id, traderId, CancellationToken.None));
        }

        return inquiry;
    }
}