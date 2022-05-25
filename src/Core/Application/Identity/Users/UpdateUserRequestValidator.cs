namespace FSH.WebApi.Application.Identity.Users;

public class UpdateUserRequestValidator : CustomValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator(IUserService userService, IStringLocalizer<UpdateUserRequestValidator> localizer)
    {
        RuleFor(u => u.Id).NotEmpty();
        RuleFor(u => u.FirstName).NotEmpty().MinimumLength(3).MaximumLength(60);
        RuleFor(u => u.LastName).NotEmpty().MinimumLength(3).MaximumLength(60);
        RuleFor(u => u.CompanyName).NotEmpty().MinimumLength(3).MaximumLength(100);

        RuleFor(u => u.Email).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
                .WithMessage(localizer["Invalid Email Address."])
            .MustAsync(async (user, email, _) => !await userService.ExistsWithEmailAsync(email, user.Id))
                .WithMessage((_, email) => string.Format(localizer["email.alreadyregistered"], email));

        RuleFor(u => u.Image)
            .SetNonNullableValidator(new FileUploadRequestValidator());

        RuleFor(u => u.PhoneNumber).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Must((_, phone, _) => PhoneNumberValidator.IsValid(phone))
                .WithMessage((_, phone) => string.Format(localizer["phone.invalid"], phone));
    }
}