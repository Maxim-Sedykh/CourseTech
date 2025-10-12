using CourseTech.Domain.Enum;
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
    private readonly Role[] _allowedRoles;

    public AllowRolesAttribute(params Role[] allowedRoles) => _allowedRoles = [.. allowedRoles];

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        bool hasRequiredRole = user.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => ConvertClaimValueToRole(c.Value))
            .Where(r => r.HasValue)
            .Any(r => _allowedRoles.Contains(r.Value));

        if (!hasRequiredRole)
        {
            context.Result = new ForbidResult();
        }
    }

    /// <summary>
    /// Пытается преобразовать строковое значение клейма в enum Role.
    /// Возвращает null, если преобразование невозможно.
    /// </summary>
    private Role? ConvertClaimValueToRole(string claimValue)
    {
        if (string.IsNullOrWhiteSpace(claimValue))
        {
            return null;
        }

        if (Enum.TryParse(claimValue, true, out Role role))
        {
            return role;
        }

        return null;
    }
}