namespace FSH.WebApi.Application.Identity.Users;

public class CreateUserRequestValidator : CustomValidator<CreateUserRequest>
{
    public CreateUserRequestValidator(IUserService userService, IStringLocalizer<CreateUserRequestValidator> localizer)
    {
        RuleFor(u => u.FirstName).Cascade(CascadeMode.Stop).NotEmpty().MinimumLength(3).MaximumLength(60);
        RuleFor(u => u.LastName).Cascade(CascadeMode.Stop).NotEmpty().MinimumLength(3).MaximumLength(60);
        RuleFor(u => u.CompanyName).Cascade(CascadeMode.Stop).NotEmpty().MinimumLength(3).MaximumLength(100);
        RuleFor(u => u.Password).Cascade(CascadeMode.Stop).NotEmpty().MinimumLength(6);
        RuleFor(u => u.ConfirmPassword).Cascade(CascadeMode.Stop).NotEmpty().Equal(u => u.Password);

        RuleFor(u => u.Email).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
                .WithMessage(localizer["Invalid Email Address."])
            .MustAsync(async (email, _) => !await userService.ExistsWithEmailAsync(email))
                .WithMessage((_, email) => string.Format(localizer["Email {0} is already registered."], email));

        RuleFor(u => u.UserName).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(6)
            .MustAsync(async (name, _) => !await userService.ExistsWithNameAsync(name))
                .WithMessage((_, name) => string.Format(localizer["Username {0} is already taken."], name));

        RuleFor(u => u.PhoneNumber).Cascade(CascadeMode.Stop)
            .MustAsync(async (phone, _) => !await userService.ExistsWithPhoneNumberAsync(phone!))
                .WithMessage((_, phone) => string.Format(localizer["Phone number {0} is already registered."], phone))
                .Unless(u => string.IsNullOrWhiteSpace(u.PhoneNumber));
    }
}