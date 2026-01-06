using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseTech.WebApi.Controllers;

/// <summary>
/// Обёртка над ControllerBase для предоставления данных для аутентификации и авторизации
/// </summary>
public class PrincipalInfoController : ControllerBase
{
    /// <summary>
    /// Идентификатор авторизованного пользователя
    /// </summary>
    protected Guid AuthorizedUserId => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId)
        ? userId
        : Guid.Empty;
}
