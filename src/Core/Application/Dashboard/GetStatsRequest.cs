namespace FSH.WebApi.Application.Dashboard;

public class GetStatsRequest : IRequest<StatsDto>
{
}

public class GetStatsRequestHandler : IRequestHandler<GetStatsRequest, StatsDto>
{
    private readonly IReadRepository<Group> _groupRepo;
    private readonly IReadRepository<Trader> _traderRepo;
    private readonly IReadRepository<Offer> _offerRepo;
    private readonly IReadRepository<Inquiry> _inquiryRepo;
    private readonly IStringLocalizer<GetStatsRequestHandler> _localizer;

    public GetStatsRequestHandler(IReadRepository<Group> groupRepo, IReadRepository<Trader> traderRepo, IReadRepository<Offer> offerRepo, IReadRepository<Inquiry> inquiryRepo, IStringLocalizer<GetStatsRequestHandler> localizer)
    {
        _groupRepo = groupRepo;
        _traderRepo = traderRepo;
        _offerRepo = offerRepo;
        _inquiryRepo = inquiryRepo;
        _localizer = localizer;
    }

    public async Task<StatsDto> Handle(GetStatsRequest request, CancellationToken cancellationToken)
    {
        var stats = new StatsDto
        {
            GroupCount = await _groupRepo.CountAsync(cancellationToken),
            TraderCount = await _traderRepo.CountAsync(cancellationToken),
            InquiryCount = await _inquiryRepo.CountAsync(cancellationToken),
            OfferCount = await _offerRepo.CountAsync(cancellationToken)
        };

        int selectedYear = DateTime.UtcNow.Year;
        double[] inquiriesFigure = new double[13];
        double[] offersFigure = new double[13];
        for (int i = 1; i <= 12; i++)
        {
            int month = i;
            var filterStartDate = new DateTime(selectedYear, month, 01).ToUniversalTime();
            var filterEndDate = new DateTime(selectedYear, month, DateTime.DaysInMonth(selectedYear, month), 23, 59, 59).ToUniversalTime(); // Monthly Based

            var inquirySpec = new EntitiesByCreatedOnBetweenSpec<Inquiry>(filterStartDate, filterEndDate);
            var offerSpec = new EntitiesByCreatedOnBetweenSpec<Offer>(filterStartDate, filterEndDate);

            inquiriesFigure[i - 1] = await _inquiryRepo.CountAsync(inquirySpec, cancellationToken);
            offersFigure[i - 1] = await _offerRepo.CountAsync(offerSpec, cancellationToken);
        }

        stats.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Inquiries"], Data = inquiriesFigure });
        stats.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Offers"], Data = offersFigure });

        return stats;
    }
}