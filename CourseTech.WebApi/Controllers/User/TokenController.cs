using Asp.Versioning;
using CourseTech.Domain.Constants.Route;
using CourseTech.Domain.Dto.Token;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers.User
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserTokenController(ITokenService tokenService) : ControllerBase
    {
        /// <summary>
        /// Обновление токена пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// 
        [Route(RouteConstants.RefreshToken)]
        [HttpPost]
        public async Task<ActionResult<DataResult<TokenDto>>> RefreshToken([FromBody] TokenDto dto)
        {
            var response = await tokenService.RefreshToken(dto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
