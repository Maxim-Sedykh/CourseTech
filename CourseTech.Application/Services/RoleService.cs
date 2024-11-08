using AutoMapper;
using CourseTech.Application.Commands.RoleCommands;
using CourseTech.Application.Queries.RoleQueries;
using CourseTech.Application.Queries.UserQueries;
using CourseTech.Application.Resources;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.Role;
using CourseTech.Domain.Dto.UserRole;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Interfaces.Validators;
using CourseTech.Domain.Result;
using MediatR;
using ILogger = Serilog.ILogger;

namespace CourseTech.Application.Services
{
    public class RoleService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IRoleValidator roleValidator,
        ICacheService cacheService,
        IMediator mediator,
        ILogger logger) : IRoleService
    {
        /// <inheritdoc/>
        public async Task<BaseResult<UserRoleDto>> AddRoleForUserAsync(UserRoleDto dto)
        {
            var user = await mediator.Send(new GetUserWithRolesByLoginQuery(dto.Login));

            if (user == null)
            {
                return BaseResult<UserRoleDto>.Failure((int)ErrorCodes.UserNotFound, ErrorMessage.UserNotFound);
            }

            var roles = user.Roles.Select(x => x.Name).ToArray();
            if (!roles.All(x => x == dto.RoleName))
            {
                var role = await mediator.Send(new GetRoleByNameQuery(dto.RoleName));
                if (role == null)
                {
                    return BaseResult<UserRoleDto>.Failure((int)ErrorCodes.RoleNotFound, ErrorMessage.RoleNotFound);
                }

                await mediator.Send(new CreateUserRoleCommand(role.Id, user.Id));

                return BaseResult<UserRoleDto>.Success(new UserRoleDto(user.Login, role.Name));
            }

            return BaseResult<UserRoleDto>.Failure((int)ErrorCodes.UserAlreadyExistThisRole, ErrorMessage.UserAlreadyExistThisRole);
        }

        /// <inheritdoc/>
        public async Task<BaseResult<RoleDto>> CreateRoleAsync(CreateRoleDto dto)
        {
            var role = await mediator.Send(new GetRoleByNameQuery(dto.RoleName));

            if (role != null)
            {
                return BaseResult<RoleDto>.Failure((int)ErrorCodes.RoleNotFound, ErrorMessage.RoleNotFound);
            }

            await mediator.Send(new CreateRoleCommand(dto.RoleName));

            await cacheService.RemoveAsync(CacheKeys.Roles);

            return BaseResult<RoleDto>.Success(mapper.Map<RoleDto>(role));
        }

        /// <inheritdoc/>
        public async Task<BaseResult<RoleDto>> DeleteRoleAsync(long id)
        {
            var role = await mediator.Send(new GetRoleByIdQuery(id));

            if (role == null)
            {
                return BaseResult<RoleDto>.Failure((int)ErrorCodes.RoleNotFound, ErrorMessage.RoleNotFound);
            }

            await mediator.Send(new DeleteRoleCommand(role));

            await cacheService.RemoveAsync(CacheKeys.Roles);

            return BaseResult<RoleDto>.Success(mapper.Map<RoleDto>(role));
        }

        /// <inheritdoc/>
        public async Task<BaseResult> UpdateRoleAsync(RoleDto dto)
        {
            var role = await mediator.Send(new GetRoleByIdQuery(dto.Id));

            if (role == null)
            {
                return BaseResult<RoleDto>.Failure((int)ErrorCodes.RoleNotFound, ErrorMessage.RoleNotFound);
            }

            await mediator.Send(new UpdateRoleCommand(role, dto.RoleName));

            await cacheService.RemoveAsync(CacheKeys.Roles);

            return BaseResult.Success();
        }

        /// <inheritdoc/>
        public async Task<BaseResult<UserRoleDto>> DeleteRoleForUserAsync(DeleteUserRoleDto dto)
        {
            var user = await mediator.Send(new GetUserWithRolesByLoginQuery(dto.Login));

            var role = user.Roles.FirstOrDefault(x => x.Id == dto.RoleId);

            var validateRoleForUserResult = roleValidator.ValidateRoleForUser(user, role);
            if (!validateRoleForUserResult.IsSuccess)
            {
                return BaseResult<UserRoleDto>.Failure((int)validateRoleForUserResult.Error.Code, validateRoleForUserResult.Error.Message);
            }

            var userRole = await mediator.Send(new GetUserRoleByIdsQuery(role.Id, user.Id));

            await mediator.Send(new DeleteUserRoleCommand(userRole));

            return BaseResult<UserRoleDto>.Success(new UserRoleDto(user.Login, role.Name));
        }

        /// <inheritdoc/>
        public async Task<BaseResult<UserRoleDto>> UpdateRoleForUserAsync(UpdateUserRoleDto dto)
        {
            var user = await mediator.Send(new GetUserWithRolesByLoginQuery(dto.Login));

            var role = await mediator.Send(new GetRoleByIdQuery(dto.FromRoleId));
            var newRoleForUser = await mediator.Send(new GetRoleByIdQuery(dto.ToRoleId));

            var validateRoleForUserResult = roleValidator.ValidateRoleForUser(user, role, newRoleForUser);
            if (!validateRoleForUserResult.IsSuccess)
            {
                return BaseResult<UserRoleDto>.Failure((int)validateRoleForUserResult.Error.Code, validateRoleForUserResult.Error.Message);
            }

            using (var transaction = await unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var userRole = await mediator.Send(new GetUserRoleByIdsQuery(role.Id, user.Id));

                    await mediator.Send(new DeleteUserRoleCommand(userRole));
                    await mediator.Send(new CreateUserRoleCommand(newRoleForUser.Id, user.Id));

                    await unitOfWork.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, ex.Message);

                    await transaction.RollbackAsync();
                }
            }


            return BaseResult<UserRoleDto>.Success(new UserRoleDto(user.Login, newRoleForUser.Name));
        }

        /// <inheritdoc/>
        public async Task<CollectionResult<RoleDto>> GetAllRoles()
        {
            var roles = await cacheService.GetOrAddToCache(
                CacheKeys.Roles,
                async () => await mediator.Send(new GetRoleDtosQuery()));

            if (roles.Length == 0)
            {
                return CollectionResult<RoleDto>.Failure((int)ErrorCodes.RolesNotFound, ErrorMessage.RolesNotFound);
            }

            return CollectionResult<RoleDto>.Success(roles);
        }
    }
}
