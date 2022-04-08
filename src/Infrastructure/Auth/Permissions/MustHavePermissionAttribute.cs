using FSH.WebApi.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace FSH.WebApi.Infrastructure.Auth.Permissions;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource) =>
        Policy = AppPermission.NameFor(action, resource);
}