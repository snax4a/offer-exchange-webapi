namespace FSH.WebApi.Application.Identity.Users;

public class UpdateUserRequestValidator : CustomValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator(IUserService userService, IStringLocalizer<UpdateUserRequestValidator> localizer)
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(u => u.Id).NotEmpty();
        RuleFor(u => u.FirstName).NotEmpty().MinimumLength(3).MaximumLength(60).NotContainForbiddenCharacters();
        RuleFor(u => u.LastName).NotEmpty().MinimumLength(3).MaximumLength(60).NotContainForbiddenCharacters();
        RuleFor(u => u.CompanyName).NotEmpty().MinimumLength(3).MaximumLength(100).NotContainForbiddenCharacters();

        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress()
                .WithMessage(localizer["Invalid Email Address."])
            .NotContainForbiddenCharacters()
            .MustAsync(async (user, email, _) => !await userService.ExistsWithEmailAsync(email, user.Id))
                .WithMessage((_, email) => string.Format(localizer["email.alreadyregistered"], email));

        RuleFor(u => u.Image)
            .SetNonNullableValidator(new FileUploadRequestValidator());

        RuleFor(u => u.PhoneNumber)
            .NotEmpty()
            .NotContainForbiddenCharacters()
            .Must((_, phone, _) => PhoneNumberValidator.IsValid(phone))
                .WithMessage((_, phone) => string.Format(localizer["phone.invalid"], phone));
    }
}