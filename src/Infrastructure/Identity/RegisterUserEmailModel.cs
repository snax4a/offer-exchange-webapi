namespace FSH.WebApi.Infrastructure.Identity;

public class RegisterUserEmailModel
{
    public string PreheaderText { get; set; } = default!;
    public string GreetingText { get; set; } = default!;
    public string MainText { get; set; } = default!;
    public string ConfirmationUrl { get; set; } = default!;
    public string ConfirmButtonText { get; set; } = default!;
    public string CopyLinkDescription { get; set; } = default!;
    public string RegardsText { get; set; } = default!;
    public string TeamText { get; set; } = default!;
    public string ReadMoreDescription { get; set; } = default!;
    public string ReadMoreLinkText { get; set; } = default!;
}