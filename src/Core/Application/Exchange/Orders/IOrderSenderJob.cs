using System.ComponentModel;

namespace FSH.WebApi.Application.Exchange.Orders;

public interface IOrderSenderJob : IScopedService
{
    [DisplayName("Send new order email to trader.")]
    Task SendAsync(Guid orderId, CancellationToken cancellationToken);
}