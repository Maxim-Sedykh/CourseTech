using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CourseTech.WebApi.Attributes
{
    /// <summary>
    /// Атрибут, для того чтобы передавать массив ролей
    /// Если пользователь имеет хоть одну роль из списка, то валидация проходит
    /// </summary>
    public class AllowRolesAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly HashSet<string> _roles;

        public AllowRolesAttribute(params string[] roles)
        {
            _roles = new HashSet<string>(roles);
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            // Проверяем, аутентифицирован ли пользователь
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Проверяем, есть ли у пользователя хотя бы одна из разрешенных ролей
            if (!_roles.Any(user.IsInRole))
            {
                context.Result = new ForbidResult();
            }

            await Task.CompletedTask;
        }
    }
}
