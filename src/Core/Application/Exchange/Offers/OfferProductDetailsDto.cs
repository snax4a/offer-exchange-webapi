namespace FSH.WebApi.Application.Exchange.Offers;

public class OfferProductDetailsDto : OfferProductDto
{
    public decimal NetValue { get; set; }
    public decimal GrossValue { get; set; }
}