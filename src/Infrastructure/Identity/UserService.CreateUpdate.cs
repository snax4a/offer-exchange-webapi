﻿using System.Security.Claims;
using FSH.WebApi.Application.Common.Exceptions;
using FSH.WebApi.Application.Common.Mailing;
using FSH.WebApi.Application.Exchange.Billing.Customers.Specifications;
using FSH.WebApi.Application.Identity.Users;
using FSH.WebApi.Core.Shared.Extensions;
using FSH.WebApi.Domain.Billing;
using FSH.WebApi.Domain.Common;
using FSH.WebApi.Domain.Identity;
using FSH.WebApi.Shared.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

namespace FSH.WebApi.Infrastructure.Identity;

internal partial class UserService
{
    /// <summary>
    /// This is used when authenticating with AzureAd.
    /// The local user is retrieved using the objectidentifier claim present in the ClaimsPrincipal.
    /// If no such claim is found, an InternalServerException is thrown.
    /// If no user is found with that ObjectId, a new one is created and populated with the values from the ClaimsPrincipal.
    /// If a role claim is present in the principal, and the user is not yet in that roll, then the user is added to that role.
    /// </summary>
    public async Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal)
    {
        string? objectId = principal.GetObjectId();
        if (string.IsNullOrWhiteSpace(objectId))
        {
            throw new InternalServerException(_localizer["Invalid objectId"]);
        }

        var user = await _userManager.Users.Where(u => u.ObjectId == objectId).FirstOrDefaultAsync()
            ?? await CreateOrUpdateFromPrincipalAsync(principal);

        if (principal.FindFirstValue(ClaimTypes.Role) is string role &&
            await _roleManager.RoleExistsAsync(role) &&
            !await _userManager.IsInRoleAsync(user, role))
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        return user.Id;
    }

    private async Task<ApplicationUser> CreateOrUpdateFromPrincipalAsync(ClaimsPrincipal principal)
    {
        string? email = principal.FindFirstValue(ClaimTypes.Upn);
        string? username = principal.GetDisplayName();
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username))
        {
            throw new InternalServerException(string.Format(_localizer["Username or Email not valid."]));
        }

        var user = await _userManager.FindByNameAsync(username);
        if (user is not null && !string.IsNullOrWhiteSpace(user.ObjectId))
        {
            throw new InternalServerException(string.Format(_localizer["Username {0} is already taken."], username));
        }

        if (user is null)
        {
            user = await _userManager.FindByEmailAsync(email);
            if (user is not null && !string.IsNullOrWhiteSpace(user.ObjectId))
            {
                throw new InternalServerException(string.Format(_localizer["Email {0} is already taken."], email));
            }
        }

        IdentityResult? result;
        if (user is not null)
        {
            user.ObjectId = principal.GetObjectId();
            result = await _userManager.UpdateAsync(user);

            await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));
        }
        else
        {
            user = new ApplicationUser
            {
                ObjectId = principal.GetObjectId(),
                FirstName = principal.FindFirstValue(ClaimTypes.GivenName),
                LastName = principal.FindFirstValue(ClaimTypes.Surname),
                CompanyName = principal.FindFirstValue(AppClaims.ComapnyName),
                Email = email,
                NormalizedEmail = email.ToUpperInvariant(),
                UserName = username,
                NormalizedUserName = username.ToUpperInvariant(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            result = await _userManager.CreateAsync(user);

            await _events.PublishAsync(new ApplicationUserCreatedEvent(user.Id));
        }

        if (!result.Succeeded)
        {
            throw new InternalServerException(_localizer["Validation Errors Occurred."], result.GetErrors(_localizer));
        }

        return user;
    }

    public async Task<string> CreateAsync(CreateUserRequest request)
    {
        var user = new ApplicationUser
        {
            Email = request.Email.StripHtml(),
            FirstName = request.FirstName.StripHtml(),
            LastName = request.LastName.StripHtml(),
            CompanyName = request.CompanyName.StripHtml(),
            PhoneNumber = request.PhoneNumber.StripHtml(),
            IsActive = true
        };

        // Identity requires username so we pass user id here
        user.UserName = user.Id;

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new InternalServerException(_localizer["Validation Errors Occurred."], result.GetErrors(_localizer));
        }

        await _userManager.AddToRoleAsync(user, AppRoles.Basic);

        var messages = new List<string> { string.Format(_localizer["User {0} Registered."], user.Email) };

        if (_securitySettings.RequireConfirmedAccount && !string.IsNullOrEmpty(user.Email))
        {
            await SendVerificationEmailAsync(user);
            messages.Add(_localizer[$"Please check {user.Email} to verify your account!"]);
        }

        await CreateCustomerForUserAsync(user);
        await _events.PublishAsync(new ApplicationUserCreatedEvent(user.Id));

        return string.Join(Environment.NewLine, messages);
    }

    public async Task UpdateAsync(UpdateUserRequest request, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_localizer["User Not Found."]);

        string strippedEmail = request.Email.StripHtml();
        string strippedCompanyName = request.CompanyName.StripHtml();
        bool shouldUpdateStripeCustomer = strippedEmail != user.Email || strippedCompanyName != user.CompanyName;

        string currentImage = user.ImageUrl ?? string.Empty;
        if (request.Image != null || request.DeleteCurrentImage)
        {
            user.ImageUrl = await _fileStorage.UploadAsync<ApplicationUser>(request.Image, FileType.Image);
            if (request.DeleteCurrentImage && !string.IsNullOrEmpty(currentImage))
            {
                string root = Directory.GetCurrentDirectory();
                _fileStorage.Remove(Path.Combine(root, currentImage));
            }
        }

        user.FirstName = request.FirstName.StripHtml();
        user.LastName = request.LastName.StripHtml();
        user.CompanyName = strippedCompanyName;
        user.Email = strippedEmail; // TODO: send verification email instead
        user.PhoneNumber = request.PhoneNumber.StripHtml();

        var result = await _userManager.UpdateAsync(user);

        await _signInManager.RefreshSignInAsync(user);

        await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));

        if (!result.Succeeded)
        {
            throw new InternalServerException(_localizer["Update profile failed"], result.GetErrors(_localizer));
        }

        if (shouldUpdateStripeCustomer)
        {
            // Update customer data in stripe
            var spec = new CustomerByUserIdSpec(Guid.Parse(user.Id));
            var customer = await _customerRepository.GetBySpecAsync(spec);
            await _stripeService.UpdateCustomer(customer!.StripeCustomerId, strippedEmail, strippedCompanyName);
        }
    }

    private async Task<string> SendVerificationEmailAsync(ApplicationUser user)
    {
        if (_clientAppSettings.AboutPageUrl is null)
            throw new InternalServerException("ClientAppSettings AboutPageUrl is missing.");

        // send verification email
        string emailVerificationUri = await GetEmailVerificationUriAsync(user);
        RegisterUserEmailModel emailModel = new RegisterUserEmailModel()
        {
            PreheaderText = _localizer["createusermail.preheader-text"],
            GreetingText = string.Format(_localizer["mail.greeting-text"], user.FirstName),
            MainText = string.Format(_localizer["createusermail.main-text"], $"<b>{user.Email}</b>"),
            ConfirmationUrl = emailVerificationUri,
            ConfirmButtonText = _localizer["createusermail.confirm-button-text"],
            CopyLinkDescription = _localizer["mail.copy-link-description"],
            RegardsText = _localizer["mail.regards-text"],
            TeamText = _localizer["mail.team-text"],
            ReadMoreDescription = _localizer["mail.read-more-description"],
            AboutPageUrl = _clientAppSettings.AboutPageUrl,
            ReadMoreLinkText = _localizer["mail.read-more-link-text"]
        };

        var mailRequest = new MailRequest(
            new List<string> { user.Email },
            _localizer["createusermail.subject"],
            _templateService.GenerateEmailTemplate("email-confirmation", emailModel));

        return _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));
    }

    private async Task<Customer> CreateCustomerForUserAsync(ApplicationUser user)
    {
        var stripeCustomer = await _stripeService.CreateCustomer(user.Email, user.CompanyName, user.Id);
        _ = stripeCustomer ?? throw new InternalServerException(_localizer["Stripe Customer Creation Failed."]);

        var customer = new Customer(Guid.Parse(user.Id), stripeCustomer.Id);
        await _customerRepository.AddAsync(customer);

        return customer;
    }
}
