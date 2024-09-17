using Asp.Versioning;
using CourseTech.Application.Validations.FluentValidations.Role;
using CourseTech.Domain.Dto.Role;
using CourseTech.Domain.Dto.UserRole;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RoleController(IRoleService roleService, CreateRoleValidator createRoleValidation, UpdateRoleValidator updateRoleValidator) : ControllerBase
    {
        [HttpPost("create-role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<Role>>> CreateRole([FromBody] CreateRoleDto dto)
        {
            var validationResult = await createRoleValidation.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await roleService.CreateRoleAsync(dto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update-role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<Role>>> UpdateRole([FromBody] RoleDto dto)
        {
            var validationResult = await updateRoleValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await roleService.UpdateRoleAsync(dto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("delete-role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<Role>>> DeleteRole(long id)
        {
            var response = await roleService.DeleteRoleAsync(id);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("add-role-for-user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<Role>>> AddRoleForUser([FromBody] UserRoleDto dto)
        {
            var response = await roleService.AddRoleForUserAsync(dto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("delete-role-for-user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<Role>>> DeleteRoleForUser([FromBody] DeleteUserRoleDto dto)
        {
            var response = await roleService.DeleteRoleForUserAsync(dto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update-role-for-user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<Role>>> UpdateRoleForUser([FromBody] UpdateUserRoleDto dto)
        {
            var response = await roleService.UpdateRoleForUserAsync(dto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("get-all-roles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CollectionResult<RoleDto>>> GetAllRoles()
        {
            var response = await roleService.GetAllRoles();
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
