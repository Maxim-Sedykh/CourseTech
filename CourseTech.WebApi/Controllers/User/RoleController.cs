using Asp.Versioning;
using CourseTech.Application.Validations.FluentValidations.Role;
using CourseTech.Domain.Constants;
using CourseTech.Domain.Constants.Route;
using CourseTech.Domain.Dto.Role;
using CourseTech.Domain.Dto.UserRole;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using CourseTech.WebApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseTech.WebApi.Controllers.User;

[AllowRoles(Roles.Admin, Roles.Moderator)]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class RoleController(IRoleService roleService, CreateRoleValidator createRoleValidation, UpdateRoleValidator updateRoleValidator) : ControllerBase
{
    [HttpPost(RouteConstants.CreateRole)]
    public async Task<ActionResult<DataResult<Role>>> CreateRole([FromBody] CreateRoleDto dto)
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

    [HttpPut(RouteConstants.UpdateRole)]
    public async Task<ActionResult<DataResult<Role>>> UpdateRole([FromBody] RoleDto dto)
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

    [HttpDelete(RouteConstants.DeleteRoleById)]
    public async Task<ActionResult<DataResult<Role>>> DeleteRole(long id)
    {
        var response = await roleService.DeleteRoleAsync(id);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPost(RouteConstants.AddRoleForUser)]
    public async Task<ActionResult<DataResult<Role>>> AddRoleForUser([FromBody] UserRoleDto dto)
    {
        var response = await roleService.AddRoleForUserAsync(dto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpDelete(RouteConstants.DeleteRoleForUser)]
    public async Task<ActionResult<DataResult<Role>>> DeleteRoleForUser([FromBody] DeleteUserRoleDto dto)
    {
        var response = await roleService.DeleteRoleForUserAsync(dto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpPut(RouteConstants.UpdateRoleForUser)]
    public async Task<ActionResult<DataResult<Role>>> UpdateRoleForUser([FromBody] UpdateUserRoleDto dto)
    {
        var response = await roleService.UpdateRoleForUserAsync(dto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [AllowAnonymous]
    [HttpGet(RouteConstants.GetAllRoles)]
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
