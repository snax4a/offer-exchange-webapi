using System.ComponentModel;

namespace FSH.WebApi.Application.Exchange.Inquiries;

public interface IInquirySenderJob : IScopedService
{
    [DisplayName("Send inquiry emails to all recipients of the inquiry.")]
    Task SendAsync(Guid inquiryId, Guid traderId, CancellationToken cancellationToken);
}