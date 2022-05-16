namespace FSH.WebApi.Infrastructure.Exchange.Offers;

public class NewOfferEmailModel
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string CompanyName { get; set; } = default!;
    public string GreetingText { get; set; } = default!;
    public string MainText1 { get; set; } = default!;
    public string MainText2 { get; set; } = default!;
    public string OfferDetailsUrl { get; set; } = default!;
    public string OfferDetailsButtonText { get; set; } = default!;
    public string CopyLinkDescription { get; set; } = default!;
    public string RegardsText { get; set; } = default!;
    public string TeamText { get; set; } = default!;
}