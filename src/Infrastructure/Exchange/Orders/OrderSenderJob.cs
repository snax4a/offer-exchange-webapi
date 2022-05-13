using FSH.WebApi.Application.Common.Exceptions;
using FSH.WebApi.Application.Common.Mailing;
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Application.Exchange.Orders;
using FSH.WebApi.Application.Exchange.Orders.Specifications;
using FSH.WebApi.Application.Identity.Users;
using FSH.WebApi.Domain.Exchange;
using FSH.WebApi.Infrastructure.ClientApp;
using FSH.WebApi.Infrastructure.Common;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FSH.WebApi.Infrastructure.Exchange.Orders;

public class OrderSenderJob : IOrderSenderJob
{
    private readonly ClientAppSettings _clientAppSettings;
    private readonly ILogger<OrderSenderJob> _logger;
    private readonly IReadRepository<Order> _orderRepo;
    private readonly PerformingContext _performingContext;
    private readonly IStringLocalizer<OrderSenderJob> _localizer;
    private readonly IUserService _userService;
    private readonly IMailService _mailService;
    private readonly IEmailTemplateService _templateService;
    private readonly IOrderTokenService _orderTokenService;

    public OrderSenderJob(
        IOptions<ClientAppSettings> clientAppSettings,
        ILogger<OrderSenderJob> logger,
        IReadRepository<Order> orderRepo,
        PerformingContext performingContext,
        IStringLocalizer<OrderSenderJob> localizer,
        IUserService userService,
        IMailService mailService,
        IEmailTemplateService templateService,
        IOrderTokenService orderTokenService)
    {
        _clientAppSettings = clientAppSettings.Value;
        _logger = logger;
        _orderRepo = orderRepo;
        _performingContext = performingContext;
        _localizer = localizer;
        _userService = userService;
        _mailService = mailService;
        _templateService = templateService;
        _orderTokenService = orderTokenService;
    }

    [Queue("orders")]
    [AutomaticRetry(Attempts = 5)]
    public async Task SendAsync(Guid orderId, CancellationToken ct)
    {
        _logger.LogInformation("Started OrderSenderJob with Id: {jobId}", _performingContext.BackgroundJob.Id);

        string? userId = _performingContext.GetJobParameter<string?>(QueryStringKeys.UserId);

        if (string.IsNullOrEmpty(userId))
            throw new InternalServerException("User not set in performing context.");

        var user = await _userService.GetAsync(userId, ct);

        if (user is null) throw new NotFoundException($"User {userId} not found.");

        var spec = new OrderByIdAndUserSpec(orderId, user.Id);
        var order = await _orderRepo.GetBySpecAsync(spec, ct);

        if (order is null) throw new NotFoundException($"Order {orderId} not found.");

        var emailModel = GetEmailModel(user, order);
        var mailRequest = new MailRequest(
            new List<string> { order.Trader.Email },
            string.Format(_localizer["newordermail.subject"], user.CompanyName),
            _templateService.GenerateEmailTemplate("new-order", emailModel));

        await _mailService.SendAsync(mailRequest, ct);

        _logger.LogInformation("New order email successfully sent to: {email}", order.Trader.Email);
    }

    private string GetOrderDetailsUri(Guid orderId, Guid traderId)
    {
        if (_clientAppSettings.BaseUrl is null) throw new InternalServerException("ClientAppSettings BaseUrl is missing.");
        string token = _orderTokenService.GenerateToken(orderId, traderId);
        var orderDetailsUri = new Uri(string.Concat($"{_clientAppSettings.BaseUrl}", "/order-details/", token));
        return orderDetailsUri.ToString();
    }

    private NewOrderEmailModel GetEmailModel(UserDetailsDto user, Order order)
    {
        if (_clientAppSettings.AboutPageUrl is null)
            throw new InternalServerException("ClientAppSettings AboutPageUrl is missing.");

        return new NewOrderEmailModel()
        {
            GreetingText = string.Format(_localizer["mail.greeting-text"], order.Trader.FirstName),
            MainText1 = string.Format(_localizer["newordermail.main-text-1"], $"<b>{user.FirstName}", $"{user.LastName}</b>", $"<b>{user.CompanyName}</b>"),
            MainText2 = string.Format(_localizer["newordermail.main-text-2"], $"<b>{user.Email}</b>"),
            MainText3 = _localizer["newordermail.main-text-3"],
            OrderDetailsUrl = GetOrderDetailsUri(order.Id, order.TraderId),
            OrderDetailsButtonText = _localizer["newordermail.orderdetails-button-text"],
            CopyLinkDescription = _localizer["mail.copy-link-description"],
            RegardsText = _localizer["mail.regards-text"],
            TeamText = _localizer["mail.team-text"],
            ReadMoreDescription = _localizer["mail.read-more-description"],
            AboutPageUrl = _clientAppSettings.AboutPageUrl,
            ReadMoreLinkText = _localizer["mail.read-more-link-text"]
        };
    }
}