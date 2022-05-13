using System.Text;
using FSH.WebApi.Application.Common.Exceptions;
using FSH.WebApi.Infrastructure.Common;
using FSH.WebApi.Shared.Multitenancy;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FSH.WebApi.Infrastructure.Identity;

internal partial class UserService
{
    private async Task<string> GetEmailVerificationUriAsync(ApplicationUser user)
    {
        EnsureValidTenant();

        var clientAppSettings = _configuration.GetSection(nameof(ClientAppSettings)).Get<ClientAppSettings>();
        if (clientAppSettings.BaseUrl is null) throw new InternalServerException("ClientAppSettings BaseUrl is missing.");

        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        string encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        Uri clientUri = new Uri(string.Concat($"{clientAppSettings.BaseUrl}", "/auth/confirm-email"));
        string verificationUri = QueryHelpers.AddQueryString(clientUri.ToString(), QueryStringKeys.UserId, user.Id);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, QueryStringKeys.Token, encodedToken);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, MultitenancyConstants.TenantIdName, _currentTenant.Id!);
        return verificationUri;
    }

    public async Task<string> ConfirmEmailAsync(string userId, string token, string tenant, CancellationToken cancellationToken)
    {
        EnsureValidTenant();

        var user = await _userManager.Users
            .Where(u => u.Id == userId && !u.EmailConfirmed)
            .FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new InternalServerException(_localizer["An error occurred while confirming E-Mail."]);

        string decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

        return result.Succeeded
            ? string.Format(_localizer["Account Confirmed for E-Mail {0}. You can now login."], user.Email)
            : throw new InternalServerException(string.Format(_localizer["An error occurred while confirming {0}"], user.Email));
    }

    public async Task<string> ConfirmPhoneNumberAsync(string userId, string code)
    {
        EnsureValidTenant();

        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new InternalServerException(_localizer["An error occurred while confirming Mobile Phone."]);

        var result = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, code);

        return result.Succeeded
            ? user.EmailConfirmed
                ? string.Format(_localizer["Account Confirmed for Phone Number {0}. You can now login."], user.PhoneNumber)
                : string.Format(_localizer["Account Confirmed for Phone Number {0}. You should confirm your E-mail before logging in."], user.PhoneNumber)
            : throw new InternalServerException(string.Format(_localizer["An error occurred while confirming {0}"], user.PhoneNumber));
    }
}