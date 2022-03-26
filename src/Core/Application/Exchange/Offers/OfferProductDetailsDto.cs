namespace FSH.WebApi.Application.Exchange.Offers;

public class OfferProductDetailsDto : OfferProductDto
{
    public long NetValue { get; set; }
    public long GrossValue { get; set; }
    public Guid OfferId { get; set; }
}