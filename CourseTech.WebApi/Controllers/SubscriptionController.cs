using Asp.Versioning;
using CourseTech.Domain.Dto.Subscription;
using CourseTech.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers;

/// <summary>
/// Управление подписками пользователей
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class SubscriptionsController : BaseApiController
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionsController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    /// <summary>
    /// Получение всех доступных подписок
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     GET /api/v1/subscriptions
    ///     
    /// Возвращает список подписок: Free, Pro, Premium
    /// </remarks>
    /// <returns>Список доступных подписок</returns>
    /// <response code="200">Успешное получение подписок</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<SubscriptionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<SubscriptionDto[]>> GetSubscriptions()
    {
        var result = await _subscriptionService.GetSubscriptionsAsync();

        return HandleDataResult(result);
    }

    /// <summary>
    /// Изменение подписки текущего пользователя
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// 
    ///     PUT /api/v1/subscriptions/change
    ///     Headers: Authorization: Bearer {token}
    ///     {
    ///         "subscriptionId": 2
    ///     }
    /// </remarks>
    /// <param name="dto">Данные для изменения подписки</param>
    /// <returns>Результат операции</returns>
    /// <response code="200">Подписка успешно изменена</response>
    /// <response code="400">Ошибка при изменении подписки</response>
    [HttpPut("change")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ChangeSubscription([FromBody] ChangeSubscriptionDto dto)
    {
        var result = await _subscriptionService.ChangeUserSubscriptionAsync(dto, AuthorizedUserId);
        return HandleResult(result);
    }
}