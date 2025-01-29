using Asp.Versioning;
using CourseTech.Application.Validations.FluentValidations.User;
using CourseTech.Domain.Constants;
using CourseTech.Domain.Constants.Route;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using CourseTech.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers.Admin
{
    [AllowRoles(Roles.Admin, Roles.Moderator)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController(IUserService userService, UpdateUserValidator updateUserValidator) : ControllerBase
    {
        [HttpGet(RouteConstants.GetUsers)]
        public async Task<ActionResult<CollectionResult<UserDto>>> GetUsersAsync()
        {
            var response = await userService.GetUsersAsync();
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [AllowRoles(Roles.Admin)]
        [HttpDelete(RouteConstants.DeleteUser)]
        public async Task<ActionResult<BaseResult>> DeleteUserAsync(Guid userId)
        {
            var response = await userService.DeleteUserAsync(userId);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(RouteConstants.GetUserById)]
        public async Task<ActionResult<DataResult<UpdateUserDto>>> GetUserByIdAsync(Guid userId)
        {
            var response = await userService.GetUserByIdAsync(userId);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut(RouteConstants.UpdateUser)]
        public async Task<ActionResult<DataResult<UpdateUserDto>>> UpdateUserAsync([FromBody] UpdateUserDto dto)
        {
            var validationResult = await updateUserValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await userService.UpdateUserDataAsync(dto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
