using FSH.WebApi.Application.Exchange.Inquiries.Specifications;
using FSH.WebApi.Application.Exchange.Offers.Specifications;
using MassTransit;

namespace FSH.WebApi.Application.Exchange.Offers;

public class CreateOfferRequest : IRequest<Guid>
{
    public string Token { get; set; } = default!;
    public string CurrencyCode { get; set; } = default!;
    public string? Freebie { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public DeliveryCostDto DeliveryCost { get; set; } = default!;
    public IList<OfferProductDto> Products { get; set; } = default!;
}

public class CreateOfferRequestValidator : CustomValidator<CreateOfferRequest>
{
    public CreateOfferRequestValidator(IOfferTokenService tokenService, IStringLocalizer<CreateOfferRequestValidator> localizer)
    {
        RuleFor(o => o.Token)
            .NotEmpty()
            .Must(token => tokenService.ValidateToken(token))
            .WithMessage(localizer["offer.invalidtoken"]);

        RuleFor(o => o.Freebie).MinimumLength(10).MaximumLength(1000);
        RuleFor(o => o.ExpirationDate).GreaterThanOrEqualTo(DateTime.UtcNow);

        RuleFor(o => o.DeliveryCost).SetValidator(new DeliveryCostValidator());

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

    public CreateOfferRequestHandler(
        IRepositoryWithEvents<Offer> offerRepo,
        IReadRepository<Inquiry> inquiryRepo,
        IOfferTokenService tokenService)
    {
        (_offerRepo, _inquiryRepo, _tokenService) = (offerRepo, inquiryRepo, tokenService);
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
        List<OfferProduct> products = new();

        foreach (OfferProductDto product in request.Products)
        {
            products.Add(new OfferProduct(
                offerId,
                product.InquiryProductId,
                request.CurrencyCode,
                product.VatRate,
                product.Quantity,
                product.NetPrice,
                product.DeliveryDate,
                product.IsReplacement,
                product.ReplacementName,
                product.Freebie));
        }

        var deliveryCost = new DeliveryCost(request.DeliveryCost.Type, request.DeliveryCost.GrossPrice, request.DeliveryCost.Description);

        var offer = new Offer(offerId, inquiryId, traderId, inquiry.CreatedBy, request.CurrencyCode, deliveryCost, request.Freebie, products);

        await _offerRepo.AddAsync(offer, ct);

        return offer.Id;
    }
}