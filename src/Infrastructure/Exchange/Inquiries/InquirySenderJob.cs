﻿using System.Text;
using FSH.WebApi.Application.Common.Exceptions;
using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Application.Common.Mailing;
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Application.Exchange.Inquiries;
using FSH.WebApi.Application.Exchange.Offers;
using FSH.WebApi.Application.Identity.Users;
using FSH.WebApi.Domain.Exchange;
using FSH.WebApi.Infrastructure.Common;
using FSH.WebApi.Infrastructure.Common.Settings;
using FSH.WebApi.Infrastructure.Exchange.Inquiries;
using Hangfire;
using Hangfire.Server;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace FSH.WebApi.Infrastructure.Catalog;

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
        var corsSettings = _configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>();
        if (corsSettings.React is null) throw new InternalServerException("React cors setting is missing.");
        string token = _offerTokenService.GenerateToken(inquiryId, traderId);
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var offerFormUri = new Uri(string.Concat($"{corsSettings.React}", "/create-offer/", token));
        return offerFormUri.ToString();
    }

    private InquiryEmailModel GetEmailModel(UserDetailsDto user, Trader trader, Guid inquiryId)
    {
        return new InquiryEmailModel()
        {
            GreetingText = string.Format(_localizer["inquirymail.greeting-text"], trader.FirstName),
            MainText1 = string.Format(_localizer["inquirymail.main-text-1"], "<b>Szymon", "Sus</b>", $"<b>{user.CompanyName}</b>"),
            MainText2 = string.Format(_localizer["inquirymail.main-text-2"], $"<b>{user.Email}</b>"),
            MainText3 = _localizer["inquirymail.main-text-3"],
            OfferFormUrl = GetOfferFormUri(inquiryId, trader.Id),
            OfferFormButtonText = _localizer["inquirymail.offer-button-text"],
            CopyLinkDescription = _localizer["inquirymail.copy-link-description"],
            RegardsText = _localizer["inquirymail.regards-text"],
            TeamText = _localizer["inquirymail.team-text"],
            ReadMoreDescription = _localizer["inquirymail.read-more-description"],
            ReadMoreLinkText = _localizer["inquirymail.read-more-link-text"]
        };
    }
}