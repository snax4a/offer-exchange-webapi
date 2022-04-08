using FSH.WebApi.Application.Comparison.DTOs;
using FSH.WebApi.Application.Comparison.Specifications;
using FSH.WebApi.Application.Exchange.Inquiries.DTOs;
using Mapster;

namespace FSH.WebApi.Application.Exchange.Comparison;

public class GetProductOffersRequest : IRequest<ProductOfferListDto>
{
    public Guid InquiryProductId { get; set; }

    public GetProductOffersRequest(Guid inquiryProductId)
        => InquiryProductId = inquiryProductId;
}

public class GetProductOffersRequestHandler : IRequestHandler<GetProductOffersRequest, ProductOfferListDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly IReadRepository<InquiryProduct> _repository;

    public GetProductOffersRequestHandler(ICurrentUser currentUser, IReadRepository<InquiryProduct> repository)
        => (_currentUser, _repository) = (currentUser, repository);

    public async Task<ProductOfferListDto> Handle(GetProductOffersRequest request, CancellationToken ct)
    {
        var spec = new InquiryProductByIdAndUserWithOffersSpec(request.InquiryProductId, _currentUser.GetUserId());
        var inquiryProduct = await _repository.GetBySpecAsync(spec, ct);

        if (inquiryProduct is null)
            throw new NotFoundException(string.Format("Inquiry product with id: {0} not found.", request.InquiryProductId));

        return new ProductOfferListDto
        {
            InquiryProduct = inquiryProduct.Adapt<InquiryProductDto>(),
            ProductOffers = inquiryProduct.OfferProducts.Adapt<List<ProductOfferDto>>()
        };
    }
}