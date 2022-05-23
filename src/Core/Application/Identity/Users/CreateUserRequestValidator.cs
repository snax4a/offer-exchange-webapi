namespace FSH.WebApi.Application.Identity.Users;

public class CreateUserRequestValidator : CustomValidator<CreateUserRequest>
{
    public CreateUserRequestValidator(IUserService userService, IStringLocalizer<CreateUserRequestValidator> localizer)
    {
        RuleFor(u => u.FirstName).NotEmpty().MinimumLength(3).MaximumLength(60);
        RuleFor(u => u.LastName).NotEmpty().MinimumLength(3).MaximumLength(60);
        RuleFor(u => u.CompanyName).NotEmpty().MinimumLength(3).MaximumLength(100);
        RuleFor(u => u.Password).NotEmpty().MinimumLength(6);
        RuleFor(u => u.ConfirmPassword).NotEmpty().Equal(u => u.Password);

        RuleFor(u => u.Email).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
                .WithMessage(localizer["Invalid Email Address."])
            .MustAsync(async (email, _) => !await userService.ExistsWithEmailAsync(email))
                .WithMessage((_, email) => string.Format(localizer["Email {0} is already registered."], email));

        RuleFor(u => u.PhoneNumber).Cascade(CascadeMode.Stop)
          .NotEmpty()
            .Must((_, phone, _) => PhoneNumberValidator.IsValid(phone))
                .WithMessage((_, phone) => string.Format(localizer["phone.invalid"], phone));
    }
}