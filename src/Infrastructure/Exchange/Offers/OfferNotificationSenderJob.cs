using FSH.WebApi.Application.Common.Exceptions;
using FSH.WebApi.Application.Common.Mailing;
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Application.Exchange.Offers;
using FSH.WebApi.Application.Exchange.Offers.Specifications;
using FSH.WebApi.Application.Identity.Users;
using FSH.WebApi.Domain.Exchange;
using FSH.WebApi.Infrastructure.ClientApp;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FSH.WebApi.Infrastructure.Exchange.Offers;

public class OfferNotificationSenderJob : IOfferNotificationSenderJob
{
    private readonly ClientAppSettings _clientAppSettings;
    private readonly ILogger<OfferNotificationSenderJob> _logger;
    private readonly IReadRepository<Offer> _offerRepo;
    private readonly PerformingContext _performingContext;
    private readonly IStringLocalizer<OfferNotificationSenderJob> _localizer;
    private readonly IUserService _userService;
    private readonly IMailService _mailService;
    private readonly IEmailTemplateService _templateService;

    public OfferNotificationSenderJob(
        IOptions<ClientAppSettings> clientAppSettings,
        ILogger<OfferNotificationSenderJob> logger,
        IReadRepository<Offer> offerRepo,
        PerformingContext performingContext,
        IStringLocalizer<OfferNotificationSenderJob> localizer,
        IUserService userService,
        IMailService mailService,
        IEmailTemplateService templateService)
    {
        _clientAppSettings = clientAppSettings.Value;
        _logger = logger;
        _offerRepo = offerRepo;
        _performingContext = performingContext;
        _localizer = localizer;
        _userService = userService;
        _mailService = mailService;
        _templateService = templateService;
    }

    [Queue("offers")]
    [AutomaticRetry(Attempts = 5)]
    public async Task NotifyUserAsync(Guid offerId, CancellationToken ct)
    {
        _logger.LogInformation("Started OfferNotificationSenderJob -> NotifyUserAsync with Id: {jobId}", _performingContext.BackgroundJob.Id);

        var spec = new OfferDetailsSpec(offerId);
        var offer = await _offerRepo.GetBySpecAsync(spec, ct);
        if (offer is null) throw new NotFoundException($"Offer {offerId} not found.");

        var user = await _userService.GetAsync(offer.UserId.ToString(), ct);
        if (user is null) throw new NotFoundException($"User {offer.UserId} not found.");

        var emailModel = GetEmailModel(user, offer);
        var mailRequest = new MailRequest(
            new List<string> { user.Email },
            string.Format(_localizer["newoffermail.subject"], $"{offer.Trader.FirstName} {offer.Trader.LastName}"),
            _templateService.GenerateEmailTemplate("new-offer", emailModel));

        await _mailService.SendAsync(mailRequest, ct);

        _logger.LogInformation("Offer notification email successfully sent to: {email}", user.Email);
    }

    private string GetOfferDetailsUrl(Guid offerId)
    {
        if (_clientAppSettings.BaseUrl is null) throw new InternalServerException("ClientAppSettings BaseUrl is missing.");
        return string.Concat($"{_clientAppSettings.BaseUrl}", "/dashboard/offers/", offerId);
    }

    private NewOfferEmailModel GetEmailModel(UserDetailsDto user, Offer offer)
    {
        string traderFullName = $"<b>{offer.Trader.FirstName} {offer.Trader.LastName}</b>";
        string fromCompanyText = string.Empty;

        if (offer.Trader.CompanyName is not null)
            fromCompanyText = string.Format(_localizer["text.from-company"], $"<b>{user.CompanyName}</b>");

        return new NewOfferEmailModel()
        {
            GreetingText = string.Format(_localizer["mail.greeting-text"], user.FirstName),
            MainText1 = string.Format(_localizer["newoffermail.main-text-1"], traderFullName, fromCompanyText, offer.Inquiry.Name),
            MainText2 = _localizer["newoffermail.main-text-2"],
            OfferDetailsUrl = GetOfferDetailsUrl(offer.Id),
            OfferDetailsButtonText = _localizer["newoffermail.offerdetails-button-text"],
            CopyLinkDescription = _localizer["mail.copy-link-description"],
            RegardsText = _localizer["mail.regards-text"],
            TeamText = _localizer["mail.team-text"],
        };
    }
}