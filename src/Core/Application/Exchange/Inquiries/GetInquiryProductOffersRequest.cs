using FSH.WebApi.Application.Exchange.Inquiries.DTOs;
using FSH.WebApi.Application.Exchange.Inquiries.Specifications;
using Mapster;

namespace FSH.WebApi.Application.Exchange.Inquiries;

public class GetInquiryProductOffersRequest : IRequest<InquiryProductOffersDto>
{
    public Guid InquiryId { get; set; }
    public Guid InquiryProductId { get; set; }

    public GetInquiryProductOffersRequest(Guid inquiryId, Guid productId)
        => (InquiryId, InquiryProductId) = (inquiryId, productId);
}

public class GetInquiryProductOffersRequestHandler : IRequestHandler<GetInquiryProductOffersRequest, InquiryProductOffersDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly IReadRepository<InquiryProduct> _repository;

    public GetInquiryProductOffersRequestHandler(ICurrentUser currentUser, IReadRepository<InquiryProduct> repository)
        => (_currentUser, _repository) = (currentUser, repository);

    public async Task<InquiryProductOffersDto> Handle(GetInquiryProductOffersRequest request, CancellationToken ct)
    {
        var spec = new InquiryProductByIdAndUserWithOffersSpec(request.InquiryProductId, _currentUser.GetUserId());
        var inquiryProduct = await _repository.GetBySpecAsync(spec, ct);

        if (inquiryProduct is null || inquiryProduct.InquiryId != request.InquiryId)
            throw new NotFoundException(string.Format("Inquiry product with id: {0} not found.", request.InquiryId));

        return new InquiryProductOffersDto
        {
            InquiryProduct = inquiryProduct.Adapt<InquiryProductDto>(),
            OfferProducts = inquiryProduct.OfferProducts.Adapt<List<OfferProductDto>>()
        };
    }
}