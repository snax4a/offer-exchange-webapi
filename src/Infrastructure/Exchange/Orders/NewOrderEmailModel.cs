namespace FSH.WebApi.Infrastructure.Exchange.Orders;

public class NewOrderEmailModel
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string CompanyName { get; set; } = default!;
    public string GreetingText { get; set; } = default!;
    public string MainText1 { get; set; } = default!;
    public string MainText2 { get; set; } = default!;
    public string MainText3 { get; set; } = default!;
    public string OrderDetailsUrl { get; set; } = default!;
    public string OrderDetailsButtonText { get; set; } = default!;
    public string CopyLinkDescription { get; set; } = default!;
    public string RegardsText { get; set; } = default!;
    public string TeamText { get; set; } = default!;
    public string ReadMoreDescription { get; set; } = default!;
    public string AboutPageUrl { get; set; } = default!;
    public string ReadMoreLinkText { get; set; } = default!;
}