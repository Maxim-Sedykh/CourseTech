using CourseTech.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseTech.WebApi.Controllers;

public abstract class BaseApiController : ControllerBase
{
    /// <summary>
    /// Идентификатор авторизованного пользователя
    /// </summary>
    protected Guid AuthorizedUserId => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId)
        ? userId
        : Guid.Empty;

    /// <summary>
    /// Обрабатывает результат операции с возвращаемым значением
    /// </summary>
    /// <typeparam name="T">Тип возвращаемых данных</typeparam>
    /// <param name="result">Результат выполнения операции</param>
    /// <returns>200 OK с данными или 400 BadRequest с ошибками</returns>
    protected ActionResult<T> HandleDataResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return BadRequest(string.Join(Environment.NewLine, result.Errors));
    }

    /// <summary>
    /// Обрабатывает результат операции без возвращаемого значения
    /// </summary>
    /// <param name="result">Результат выполнения операции</param>
    /// <returns>200 OK или 400 BadRequest с ошибками</returns>
    protected ActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(string.Join(Environment.NewLine, result.Errors));
    }
}
