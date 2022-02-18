namespace FSH.WebApi.Domain.Exchange;

public class InquiryRecipient
{
    public Guid InquiryId { get; private set; }
    public virtual Inquiry Inquiry { get; private set; } = default!;
    public Guid TraderId { get; private set; }
    public virtual Trader Trader { get; private set; } = default!;

    public InquiryRecipient(Guid inquiryId, Guid traderId)
    {
        InquiryId = inquiryId;
        TraderId = traderId;
    }
}