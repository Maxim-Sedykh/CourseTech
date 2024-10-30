using Asp.Versioning;
using CourseTech.Application.Validations.FluentValidations.Auth;
using CourseTech.Domain.Constants.Route;
using CourseTech.Domain.Dto.Auth;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers.User
{
    [ApiController]
    [ApiVersion("1.0")]
    [AllowAnonymous]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController(IAuthService authService,
        LoginUserValidator loginUserValidator,
        RegisterUserValidator registerUserValidator) : ControllerBase
    {
        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost(RouteConstants.Register)]
        public async Task<ActionResult<BaseResult>> Register([FromBody] RegisterUserDto dto)
        {
            var validationResult = await registerUserValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await authService.Register(dto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost(RouteConstants.Login)]
        public async Task<ActionResult<BaseResult>> Login([FromBody] LoginUserDto dto)
        {
            var validationResult = await loginUserValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await authService.Login(dto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
