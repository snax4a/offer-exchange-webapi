using FSH.WebApi.Application.Exchange.Inquiries.DTOs;
using FSH.WebApi.Application.Exchange.Inquiries.Specifications;
using FSH.WebApi.Application.Exchange.Offers;
using FSH.WebApi.Application.Exchange.Offers.DTOs;
using FSH.WebApi.Application.Exchange.Offers.Specifications;
using FSH.WebApi.Application.Identity.Users;
using Mapster;

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
    private readonly IUserService _userService;
    private readonly IOfferTokenService _tokenService;
    private readonly IStringLocalizer<GetInquiryForOfferRequestHandler> _localizer;

    public GetInquiryForOfferRequestHandler(
        IReadRepository<Inquiry> inquiryRepo,
        IReadRepository<Offer> offerRepo,
        IUserService userService,
        IOfferTokenService tokenService,
        IStringLocalizer<GetInquiryForOfferRequestHandler> localizer)
    {
        _inquiryRepo = inquiryRepo;
        _offerRepo = offerRepo;
        _userService = userService;
        _tokenService = tokenService;
        _localizer = localizer;
    }

    public async Task<InquiryForOfferDto> Handle(GetInquiryForOfferRequest request, CancellationToken ct)
    {
        // Decode inquiryId and traderId from offer token
        (Guid inquiryId, Guid traderId) = _tokenService.DecodeToken(request.OfferToken);

        ISpecification<Inquiry, InquiryDetailsDto> spec = new InquiryDetailsSpec(inquiryId);
        var inquiry = await _inquiryRepo.GetBySpecAsync(spec, ct);

        if (inquiry is null)
            throw new NotFoundException(string.Format(_localizer["inquiry.notfound"], inquiryId));

        var trader = inquiry.Recipients.FirstOrDefault(t => t.Id == traderId);

        if (trader is null)
            throw new NotFoundException(string.Format(_localizer["trader.notfound"], traderId));

        var user = await _userService.GetAsync(inquiry.CreatedBy.ToString(), ct);

        ISpecification<Offer, OfferDetailsDto> offerSpec = new OfferByInquiryAndTraderSpec(inquiryId, traderId);
        var offer = await _offerRepo.GetBySpecAsync(offerSpec, ct);

        return new InquiryForOfferDto()
        {
            Id = inquiry.Id,
            ReferenceNumber = inquiry.ReferenceNumber,
            Title = inquiry.Title,
            ShippingAddress = inquiry.ShippingAddress,
            CreatedOn = inquiry.CreatedOn,
            Trader = trader,
            Creator = user.Adapt<UserDto>(),
            Products = inquiry.Products,
            Offer = offer
        };
    }
}