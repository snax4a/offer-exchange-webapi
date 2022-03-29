namespace FSH.WebApi.Infrastructure.Identity;

public class ResetPasswordEmailModel
{
    public string PreheaderText { get; set; } = default!;
    public string GreetingText { get; set; } = default!;
    public string MainText { get; set; } = default!;
    public string PasswordResetUrl { get; set; } = default!;
    public string ResetButtonText { get; set; } = default!;
    public string CopyLinkDescription { get; set; } = default!;
    public string RegardsText { get; set; } = default!;
    public string TeamText { get; set; } = default!;
}