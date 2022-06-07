namespace FSH.WebApi.Application.Identity.Tokens;

public record TokenRequest(string Email, string Password);

public class TokenRequestValidator : CustomValidator<TokenRequest>
{
    public TokenRequestValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(p => p.Email).NotEmpty().EmailAddress().WithMessage("Invalid Email Address.");
        RuleFor(p => p.Password).NotEmpty();
    }
}