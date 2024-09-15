using AutoMapper;
using CourseTech.Application.Resources;
using CourseTech.Domain.Dto.Lesson.Test;
using CourseTech.Domain.Dto.Role;
using CourseTech.Domain.Dto.UserRole;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Interfaces.Validators;
using CourseTech.Domain.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace CourseTech.Application.Services
{
    public class RoleService(IUnitOfWork unitOfWork, IMapper mapper, IRoleValidator roleValidator) : IRoleService
    {
        /// <inheritdoc/>
        public async Task<BaseResult<UserRoleDto>> AddRoleForUserAsync(UserRoleDto dto)
        {
            var user = await unitOfWork.Users.GetAll()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Login == dto.Login);

            if (user == null)
            {
                return BaseResult<UserRoleDto>.Failure((int)ErrorCodes.UserNotFound, ErrorMessage.UserNotFound);
            }

            var roles = user.Roles.Select(x => x.Name).ToArray();
            if (!roles.All(x => x == dto.RoleName))
            {
                var role = await unitOfWork.Roles.GetAll().FirstOrDefaultAsync(x => x.Name == dto.RoleName);
                if (role == null)
                {
                    return BaseResult<UserRoleDto>.Failure((int)ErrorCodes.RoleNotFound, ErrorMessage.RoleNotFound);
                }

                UserRole userRole = new UserRole()
                {
                    RoleId = role.Id,
                    UserId = user.Id
                };

                await unitOfWork.UserRoles.CreateAsync(userRole);
                await unitOfWork.SaveChangesAsync();

                return BaseResult<UserRoleDto>.Success(new UserRoleDto(user.Login, role.Name));
            }

            return BaseResult<UserRoleDto>.Failure((int)ErrorCodes.UserAlreadyExistThisRole, ErrorMessage.UserAlreadyExistThisRole);
        }

        /// <inheritdoc/>
        public async Task<BaseResult<RoleDto>> CreateRoleAsync(CreateRoleDto dto)
        {
            var role = await unitOfWork.Roles.GetAll().FirstOrDefaultAsync(x => x.Name == dto.Name);

            if (role != null)
            {
                return BaseResult<RoleDto>.Failure((int)ErrorCodes.RoleNotFound, ErrorMessage.RoleNotFound);
            }

            role = new Role()
            {
                Name = dto.Name,
            };

            await unitOfWork.Roles.CreateAsync(role);
            await unitOfWork.SaveChangesAsync();

            return BaseResult<RoleDto>.Success(mapper.Map<RoleDto>(role));
        }

        /// <inheritdoc/>
        public async Task<BaseResult<RoleDto>> DeleteRoleAsync(long id)
        {
            var role = await unitOfWork.Roles.GetAll().FirstOrDefaultAsync(x => x.Id == id);

            if (role == null)
            {
                return BaseResult<RoleDto>.Failure((int)ErrorCodes.RoleNotFound, ErrorMessage.RoleNotFound);
            }

            unitOfWork.Roles.Remove(role);
            await unitOfWork.SaveChangesAsync();

            return BaseResult<RoleDto>.Success(mapper.Map<RoleDto>(role));
        }

        /// <inheritdoc/>
        public async Task<BaseResult<RoleDto>> UpdateRoleAsync(RoleDto dto)
        {
            var role = await unitOfWork.Roles.GetAll().FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (role == null)
            {
                return BaseResult<RoleDto>.Failure((int)ErrorCodes.RoleNotFound, ErrorMessage.RoleNotFound);
            }

            role.Name = dto.Name;

            var updatedRole = unitOfWork.Roles.Update(role);
            await unitOfWork.SaveChangesAsync();

            return BaseResult<RoleDto>.Success(mapper.Map<RoleDto>(role));
        }

        /// <inheritdoc/>
        public async Task<BaseResult<UserRoleDto>> DeleteRoleForUserAsync(DeleteUserRoleDto dto)
        {
            var user = await unitOfWork.Users.GetAll()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Login == dto.Login);

            var role = user.Roles.FirstOrDefault(x => x.Id == dto.RoleId);

            var validateRoleForUserResult = roleValidator.ValidateRoleForUser(user, role);
            if (!validateRoleForUserResult.IsSuccess)
            {
                return BaseResult<UserRoleDto>.Failure((int)validateRoleForUserResult.Error.Code, validateRoleForUserResult.Error.Message);
            }

            var userRole = await unitOfWork.UserRoles.GetAll()
                .Where(x => x.RoleId == role.Id)
                .FirstOrDefaultAsync(x => x.UserId == user.Id);

            unitOfWork.UserRoles.Remove(userRole);
            await unitOfWork.SaveChangesAsync();

            return BaseResult<UserRoleDto>.Success(new UserRoleDto(user.Login, role.Name));
        }

        /// <inheritdoc/>
        public async Task<BaseResult<UserRoleDto>> UpdateRoleForUserAsync(UpdateUserRoleDto dto)
        {
            var user = await unitOfWork.Users.GetAll()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Login == dto.Login);

            var role = user.Roles.FirstOrDefault(x => x.Id == dto.FromRoleId);

            var newRoleForUser = await unitOfWork.Roles.GetAll().FirstOrDefaultAsync(x => x.Id == dto.ToRoleId);

            var validateRoleForUserResult = roleValidator.ValidateRoleForUser(user, role, newRoleForUser);
            if (!validateRoleForUserResult.IsSuccess)
            {
                return BaseResult<UserRoleDto>.Failure((int)validateRoleForUserResult.Error.Code, validateRoleForUserResult.Error.Message);
            }

            using (var transaction = await unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var userRole = await unitOfWork.UserRoles
                        .GetAll()
                        .FirstAsync(x => x.UserId == user.Id && x.RoleId == role.Id);

                    unitOfWork.UserRoles.Remove(userRole);

                    var newUserRole = new UserRole()
                    {
                        UserId = user.Id,
                        RoleId = newRoleForUser.Id,
                    };
                    await unitOfWork.UserRoles.CreateAsync(newUserRole);
                    await unitOfWork.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                }
            }


            return BaseResult<UserRoleDto>.Success(new UserRoleDto(user.Login, newRoleForUser.Name));
        }

        public async Task<CollectionResult<RoleDto>> GetAllRoles()
        {
            var roles = await unitOfWork.Roles
                        .GetAll()
                        .Select(r => mapper.Map<RoleDto>(r))
                        .ToArrayAsync();

            if (roles.Length == 0)
            {
                return CollectionResult<RoleDto>.Failure((int)ErrorCodes.RolesNotFound, ErrorMessage.RolesNotFound);
            }

            return CollectionResult<RoleDto>.Success(roles);
        }
    }
}
