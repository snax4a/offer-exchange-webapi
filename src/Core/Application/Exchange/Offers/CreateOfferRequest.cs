using FSH.WebApi.Application.Exchange.Inquiries.Specifications;
using FSH.WebApi.Application.Exchange.Offers.DTOs;
using FSH.WebApi.Application.Exchange.Offers.Specifications;
using MassTransit;

namespace FSH.WebApi.Application.Exchange.Offers;

public class CreateOfferRequest : IRequest<Guid>
{
    public string Token { get; set; } = default!;
    public string CurrencyCode { get; set; } = default!;
    public string? Freebie { get; set; }
    public DateOnly? ExpirationDate { get; set; }
    public DeliveryCostType DeliveryCostType { get; set; }
    public long DeliveryCostGrossPrice { get; set; }
    public string? DeliveryCostDescription { get; set; }
    public IList<OfferProductDto> Products { get; set; } = default!;
}

public class CreateOfferRequestValidator : CustomValidator<CreateOfferRequest>
{
    public CreateOfferRequestValidator(IOfferTokenService tokenService, IStringLocalizer<CreateOfferRequestValidator> localizer)
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(o => o.Token)
            .NotEmpty()
            .Must(token => tokenService.ValidateToken(token))
            .WithMessage(localizer["offer.invalidtoken"]);

        RuleFor(p => p.CurrencyCode).NotEmpty().Length(3);
        RuleFor(o => o.Freebie).MinimumLength(10).MaximumLength(1000);
        RuleFor(o => o.ExpirationDate).GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow));

        RuleFor(o => o.DeliveryCostType).NotEmpty();
        RuleFor(o => o.DeliveryCostGrossPrice).NotNull();
        RuleFor(o => o.DeliveryCostDescription)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(2000)
            .When(o => o.DeliveryCostType == DeliveryCostType.Variable);

        RuleFor(o => o.Products)
            .Must(products => products.Count > 0)
            .WithMessage(localizer["offer.noproducts"]);

        RuleForEach(i => i.Products).SetValidator(new OfferProductValidator());
    }
}

public class CreateOfferRequestHandler : IRequestHandler<CreateOfferRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Offer> _offerRepo;
    private readonly IReadRepository<Inquiry> _inquiryRepo;
    private readonly IOfferTokenService _tokenService;
    private readonly IJobService _jobService;

    public CreateOfferRequestHandler(
        IRepositoryWithEvents<Offer> offerRepo,
        IReadRepository<Inquiry> inquiryRepo,
        IOfferTokenService tokenService,
        IJobService jobService)
    {
        _offerRepo = offerRepo;
        _inquiryRepo = inquiryRepo;
        _tokenService = tokenService;
        _jobService = jobService;
    }

    public async Task<Guid> Handle(CreateOfferRequest request, CancellationToken ct)
    {
        (Guid inquiryId, Guid traderId) = _tokenService.DecodeToken(request.Token);

        var inquiry = await _inquiryRepo.GetBySpecAsync(new InquiryByIdAndTraderSpec(inquiryId, traderId), ct);

        // Check if inquiry exists and if the trader is its recipient
        if (inquiry is null)
        {
            throw new ConflictException("Inquiry or recipient incorrect.");
        }

        // Check if trader already placed offer to that inquiry
        if (await _offerRepo.GetBySpecAsync(new OfferByInquiryAndTraderSpec(inquiryId, traderId), ct) is not null)
        {
            throw new ConflictException("Offer already exists.");
        }

        Guid offerId = NewId.Next().ToGuid();

        var products = request.Products.Select(product => new OfferProduct(
            offerId,
            product.InquiryProduct.Id,
            request.CurrencyCode,
            product.VatRate,
            product.Quantity,
            product.NetPrice,
            product.DeliveryDate,
            product.IsReplacement,
            product.ReplacementName,
            product.Freebie)).ToList();

        var deliveryCost = new DeliveryCost(request.DeliveryCostType, request.DeliveryCostGrossPrice, request.DeliveryCostDescription);

        var offer = new Offer(
            offerId,
            inquiryId,
            traderId,
            inquiry.CreatedBy,
            request.ExpirationDate,
            request.CurrencyCode,
            deliveryCost,
            request.Freebie,
            products);

        await _offerRepo.AddAsync(offer, ct);

        _jobService.Enqueue<IOfferNotificationSenderJob>(x => x.NotifyUserAsync(offer.Id, CancellationToken.None));

        return offer.Id;
    }
}