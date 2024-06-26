﻿using FSH.WebApi.Application.Common.Exceptions;
using FSH.WebApi.Application.Common.Mailing;
using FSH.WebApi.Application.Identity.Users.Password;
using FSH.WebApi.Infrastructure.Common;
using Microsoft.AspNetCore.WebUtilities;

namespace FSH.WebApi.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<string> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        EnsureValidTenant();

        var user = await _userManager.FindByEmailAsync(request.Email.Normalize());
        if (user is null || !await _userManager.IsEmailConfirmedAsync(user))
        {
            // Don't reveal that the user does not exist or is not confirmed
            throw new InternalServerException(_localizer["An Error has occurred!"]);
        }

        string passwordResetUrl = await GetPasswordResetUrlAsync(user);
        SendResetPasswordMail(user, passwordResetUrl);
        return _localizer["Password reset link has been sent to your email address."];
    }

    public async Task<string> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email?.Normalize());

        // Don't reveal that the user does not exist
        _ = user ?? throw new InternalServerException(_localizer["An Error has occurred!"]);

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

        return result.Succeeded
            ? _localizer["Password Reset Successful!"]
            : throw new InternalServerException(_localizer["An Error has occurred!"]);
    }

    public async Task ChangePasswordAsync(ChangePasswordRequest model, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_localizer["User Not Found."]);

        var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

        if (!result.Succeeded)
        {
            throw new ConflictException(_localizer["Change password failed"], result.GetErrors(_localizer));
        }
    }

    private async Task<string> GetPasswordResetUrlAsync(ApplicationUser user)
    {
        if (_clientAppSettings.BaseUrl is null) throw new InternalServerException("ClientAppSettings BaseUrl is missing.");

        // For more information on how to enable account confirmation and password reset please
        // visit https://go.microsoft.com/fwlink/?LinkID=532713
        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        string url = new Uri(string.Concat($"{_clientAppSettings.BaseUrl}", "/auth/reset-password/")).ToString();
        return QueryHelpers.AddQueryString(url, QueryStringKeys.Token, token);
    }

    private string SendResetPasswordMail(ApplicationUser user, string passwordResetUrl)
    {
        ResetPasswordEmailModel emailModel = new ResetPasswordEmailModel()
        {
            PreheaderText = _localizer["resetpasswordmail.main-text"],
            GreetingText = string.Format(_localizer["mail.greeting-text"], user.FirstName),
            MainText = _localizer["resetpasswordmail.main-text"],
            PasswordResetUrl = passwordResetUrl,
            ResetButtonText = _localizer["resetpasswordmail.reset-button-text"],
            CopyLinkDescription = _localizer["mail.copy-link-description"],
            RegardsText = _localizer["mail.regards-text"],
            TeamText = _localizer["mail.team-text"],
        };

        var mailRequest = new MailRequest(
            new List<string> { user.Email },
            _localizer["resetpasswordmail.subject"],
            _templateService.GenerateEmailTemplate("reset-password", emailModel));

        return _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));
    }
}