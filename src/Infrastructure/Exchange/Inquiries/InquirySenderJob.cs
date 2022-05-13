using System.Text;
using FSH.WebApi.Application.Common.Exceptions;
using FSH.WebApi.Application.Common.Mailing;
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Application.Exchange.Inquiries;
using FSH.WebApi.Application.Exchange.Offers;
using FSH.WebApi.Application.Identity.Users;
using FSH.WebApi.Domain.Exchange;
using FSH.WebApi.Infrastructure.Common;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace FSH.WebApi.Infrastructure.Exchange.Inquiries;

public class InquirySenderJob : IInquirySenderJob
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<InquirySenderJob> _logger;
    private readonly IReadRepository<Trader> _traderRepo;
    private readonly PerformingContext _performingContext;
    private readonly IStringLocalizer<InquirySenderJob> _localizer;
    private readonly IUserService _userService;
    private readonly IMailService _mailService;
    private readonly IEmailTemplateService _templateService;
    private readonly IOfferTokenService _offerTokenService;

    public InquirySenderJob(
        IConfiguration configuration,
        ILogger<InquirySenderJob> logger,
        IReadRepository<Trader> traderRepo,
        PerformingContext performingContext,
        IStringLocalizer<InquirySenderJob> localizer,
        IUserService userService,
        IMailService mailService,
        IEmailTemplateService templateService,
        IOfferTokenService offerTokenService)
    {
        _configuration = configuration;
        _logger = logger;
        _traderRepo = traderRepo;
        _performingContext = performingContext;
        _localizer = localizer;
        _userService = userService;
        _mailService = mailService;
        _templateService = templateService;
        _offerTokenService = offerTokenService;
    }

    [Queue("inquiries")]
    [AutomaticRetry(Attempts = 5)]
    public async Task SendAsync(Guid inquiryId, Guid traderId, CancellationToken ct)
    {
        _logger.LogInformation("Started InquirySenderJob with Id: {jobId}", _performingContext.BackgroundJob.Id);

        string? userId = _performingContext.GetJobParameter<string?>(QueryStringKeys.UserId);

        if (string.IsNullOrEmpty(userId))
            throw new InternalServerException("User not set in performing context.");

        var user = await _userService.GetAsync(userId, ct);

        if (user is null) throw new NotFoundException($"User {userId} not found.");

        var trader = await _traderRepo.GetByIdAsync(traderId, ct);

        if (trader is null) throw new NotFoundException($"Trader {traderId} not found.");

        var emailModel = GetEmailModel(user, trader, inquiryId);
        var mailRequest = new MailRequest(
            new List<string> { trader.Email },
            string.Format(_localizer["inquirymail.subject"], user.CompanyName),
            _templateService.GenerateEmailTemplate("new-inquiry", emailModel));

        await _mailService.SendAsync(mailRequest, ct);

        _logger.LogInformation("Inquiry email successfully sent to: {email}", trader.Email);
    }

    private string GetOfferFormUri(Guid inquiryId, Guid traderId)
    {
        var clientAppSettings = _configuration.GetSection(nameof(ClientAppSettings)).Get<ClientAppSettings>();
        if (clientAppSettings.BaseUrl is null) throw new InternalServerException("ClientAppSettings BaseUrl is missing.");
        string token = _offerTokenService.GenerateToken(inquiryId, traderId);
        var offerFormUri = new Uri(string.Concat($"{clientAppSettings.BaseUrl}", "/create-offer/", token));
        return offerFormUri.ToString();
    }

    private InquiryEmailModel GetEmailModel(UserDetailsDto user, Trader trader, Guid inquiryId)
    {
        return new InquiryEmailModel()
        {
            GreetingText = string.Format(_localizer["mail.greeting-text"], trader.FirstName),
            MainText1 = string.Format(_localizer["inquirymail.main-text-1"], $"<b>{user.FirstName}", $"{user.LastName}</b>", $"<b>{user.CompanyName}</b>"),
            MainText2 = string.Format(_localizer["inquirymail.main-text-2"], $"<b>{user.Email}</b>"),
            MainText3 = _localizer["inquirymail.main-text-3"],
            OfferFormUrl = GetOfferFormUri(inquiryId, trader.Id),
            OfferFormButtonText = _localizer["inquirymail.offer-button-text"],
            CopyLinkDescription = _localizer["mail.copy-link-description"],
            RegardsText = _localizer["mail.regards-text"],
            TeamText = _localizer["mail.team-text"],
            ReadMoreDescription = _localizer["mail.read-more-description"],
            ReadMoreLinkText = _localizer["mail.read-more-link-text"]
        };
    }
}