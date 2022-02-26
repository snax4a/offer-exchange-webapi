using FSH.WebApi.Application.Exchange.Inquiries.Specifications;

namespace FSH.WebApi.Application.Exchange.Inquiries;

public class GetInquiryRequest : IRequest<InquiryDetailsDto>
{
    public Guid Id { get; set; }

    public GetInquiryRequest(Guid id) => Id = id;
}

public class GetInquiryRequestHandler : IRequestHandler<GetInquiryRequest, InquiryDetailsDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly IRepository<Inquiry> _repository;
    private readonly IStringLocalizer<GetInquiryRequestHandler> _localizer;

    public GetInquiryRequestHandler(
        ICurrentUser currentUser,
        IRepository<Inquiry> repository,
        IStringLocalizer<GetInquiryRequestHandler> localizer)
    {
        (_currentUser, _repository, _localizer) = (currentUser, repository, localizer);
    }

    public async Task<InquiryDetailsDto> Handle(GetInquiryRequest request, CancellationToken cancellationToken)
    {
        ISpecification<Inquiry, InquiryDetailsDto> spec = new InquiryDetailsSpec(request.Id, _currentUser.GetUserId());
        var inquiry = await _repository.GetBySpecAsync(spec, cancellationToken);

        if (inquiry is not null) return inquiry;

        throw new NotFoundException(string.Format(_localizer["inquiry.notfound"], request.Id));
    }
}