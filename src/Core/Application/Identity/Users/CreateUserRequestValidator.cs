namespace FSH.WebApi.Application.Identity.Users;

public class CreateUserRequestValidator : CustomValidator<CreateUserRequest>
{
    public CreateUserRequestValidator(IUserService userService, IStringLocalizer<CreateUserRequestValidator> localizer)
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(u => u.FirstName).NotEmpty().MinimumLength(3).MaximumLength(60).NotContainForbiddenCharacters();
        RuleFor(u => u.LastName).NotEmpty().MinimumLength(3).MaximumLength(60).NotContainForbiddenCharacters();
        RuleFor(u => u.CompanyName).NotEmpty().MinimumLength(3).MaximumLength(100).NotContainForbiddenCharacters();
        RuleFor(u => u.Password).NotEmpty().MinimumLength(6);
        RuleFor(u => u.ConfirmPassword).NotEmpty().Equal(u => u.Password);

        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress()
                .WithMessage(localizer["Invalid Email Address."])
                .NotContainForbiddenCharacters()
            .MustAsync(async (email, _) => !await userService.ExistsWithEmailAsync(email))
                .WithMessage((_, email) => string.Format(localizer["email.alreadyregistered"], email));

        RuleFor(u => u.PhoneNumber)
          .NotEmpty()
          .NotContainForbiddenCharacters()
            .Must((_, phone, _) => PhoneNumberValidator.IsValid(phone))
                .WithMessage((_, phone) => string.Format(localizer["phone.invalid"], phone));
    }
}