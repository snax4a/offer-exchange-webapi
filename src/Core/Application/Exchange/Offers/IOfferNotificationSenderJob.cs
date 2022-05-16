using System.ComponentModel;

namespace FSH.WebApi.Application.Exchange.Offers;

public interface IOfferNotificationSenderJob : IScopedService
{
    [DisplayName("Send new offer notification email to the user.")]
    Task NotifyUserAsync(Guid offerId, CancellationToken cancellationToken);
}