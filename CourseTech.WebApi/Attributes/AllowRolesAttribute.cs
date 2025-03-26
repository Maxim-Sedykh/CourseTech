using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace CourseTech.WebApi.Attributes;

/// <summary>
/// Атрибут, для того чтобы передавать массив ролей
/// Если пользователь имеет хоть одну роль из списка, то валидация проходит
/// </summary>
public class AllowRolesAttribute : Attribute, IAuthorizationFilter
{
    private readonly HashSet<string> _roles;

    public AllowRolesAttribute(params string[] roles) => _roles = [.. roles];

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        bool authorized = user.Claims.Any(c => c.Type == ClaimTypes.Role && _roles.Contains(c.Value, StringComparer.OrdinalIgnoreCase));

        if (!authorized)
        {
            context.Result = new ForbidResult();
        }
    }
}