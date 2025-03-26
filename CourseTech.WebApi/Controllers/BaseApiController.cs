using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseTech.WebApi.Controllers;

public class BaseApiController : ControllerBase
{
    /// <summary>
    /// Идентификатор авторизованного пользователя
    /// </summary>
    protected Guid AuthorizedUserId => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId)
        ? userId
        : Guid.Empty;
}
