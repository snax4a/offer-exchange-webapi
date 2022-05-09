namespace FSH.WebApi.Application.Identity.Users;

public class ConfirmUserEmailRequest
{
    public string UserId { get; set; } = default!;
    public string Tenant { get; set; } = default!;
    public string Token { get; set; } = default!;
}
