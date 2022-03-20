using FSH.WebApi.Application.Exchange.Inquiries.Specifications;
using FSH.WebApi.Application.Exchange.Offers;
using FSH.WebApi.Application.Exchange.Offers.Specifications;

namespace FSH.WebApi.Application.Exchange.Inquiries;

public class GetInquiryForOfferRequest : IRequest<InquiryForOfferDto>
{
    public string OfferToken { get; set; }

    public GetInquiryForOfferRequest(string offerToken) => OfferToken = offerToken;
}

public class GetInquiryForOfferRequestHandler : IRequestHandler<GetInquiryForOfferRequest, InquiryForOfferDto>
{
    private readonly IReadRepository<Inquiry> _inquiryRepo;
    private readonly IReadRepository<Offer> _offerRepo;
    private readonly IOfferTokenService _tokenService;
    private readonly IStringLocalizer<GetInquiryForOfferRequestHandler> _localizer;

    public GetInquiryForOfferRequestHandler(
        IReadRepository<Inquiry> inquiryRepo,
        IReadRepository<Offer> offerRepo,
        IOfferTokenService tokenService,
        IStringLocalizer<GetInquiryForOfferRequestHandler> localizer)
    {
        (_inquiryRepo, _offerRepo, _tokenService, _localizer) = (inquiryRepo, offerRepo, tokenService, localizer);
    }

    public async Task<InquiryForOfferDto> Handle(GetInquiryForOfferRequest request, CancellationToken ct)
    {
        // Decode inquiryId and traderId from offer token
        (Guid inquiryId, Guid traderId) = _tokenService.DecodeToken(request.OfferToken);

        // Check if trader already placed offer to that inquiry
        if (await _offerRepo.AnyAsync(new OfferByInquiryAndTraderSpec(inquiryId, traderId), ct))
        {
            throw new ConflictException("Offer already exists.");
        }

        ISpecification<Inquiry, InquiryDetailsDto> spec = new InquiryDetailsSpec(inquiryId);
        var inquiry = await _inquiryRepo.GetBySpecAsync(spec, ct);

        if (inquiry is null)
            throw new NotFoundException(string.Format(_localizer["inquiry.notfound"], inquiryId));

        var trader = inquiry.Recipients.FirstOrDefault(t => t.Id == traderId);

        if (trader is null)
            throw new NotFoundException(string.Format(_localizer["trader.notfound"], traderId));

        return new InquiryForOfferDto()
        {
            Id = inquiry.Id,
            ReferenceNumber = inquiry.ReferenceNumber,
            Title = inquiry.Title,
            CreatedOn = inquiry.CreatedOn,
            Trader = trader,
            Products = inquiry.Products
        };
    }
}