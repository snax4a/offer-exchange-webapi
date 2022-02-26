namespace FSH.WebApi.Application.Identity.Users;

public class UpdateUserRequestValidator : CustomValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator(IUserService userService, IStringLocalizer<UpdateUserRequestValidator> localizer)
    {
        RuleFor(u => u.Id).NotEmpty();
        RuleFor(u => u.FirstName).NotEmpty().MinimumLength(3).MaximumLength(60);
        RuleFor(u => u.LastName).NotEmpty().MinimumLength(3).MaximumLength(60);
        RuleFor(u => u.CompanyName).NotEmpty().MinimumLength(3).MaximumLength(100);

        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress()
                .WithMessage(localizer["Invalid Email Address."])
            .MustAsync(async (user, email, _) => !await userService.ExistsWithEmailAsync(email, user.Id))
                .WithMessage((_, email) => string.Format(localizer["Email {0} is already registered."], email));

        RuleFor(u => u.Image)
            .SetNonNullableValidator(new FileUploadRequestValidator());

        RuleFor(u => u.PhoneNumber).Cascade(CascadeMode.Stop)
            .MustAsync(async (user, phone, _) => !await userService.ExistsWithPhoneNumberAsync(phone!, user.Id))
                .WithMessage((_, phone) => string.Format(localizer["Phone number {0} is already registered."], phone))
                .Unless(u => string.IsNullOrWhiteSpace(u.PhoneNumber));
    }
}