using AutoMapper;
using CourseTech.Application.Resources;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.Services
{
    public class UserService(IUnitOfWork unitOfWork, IMapper mapper) : IUserService
    {
        public async Task<BaseResult> DeleteUserAsync(Guid userId)
        {
            var user = await unitOfWork.Users.GetAll().FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null)
            {
                return BaseResult.Failure((int)ErrorCodes.UserNotFound, ErrorMessage.UserNotFound);
            }

            var userProfile = await unitOfWork.UserProfiles.GetAll().FirstOrDefaultAsync(x => x.UserId == user.Id);

            if (userProfile is null)
            {
                return BaseResult.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
            }

            using (var transaction = await unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    unitOfWork.Users.Remove(user);
                    unitOfWork.UserProfiles.Remove(userProfile);

                    await unitOfWork.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                }
            }

            return BaseResult.Success();
        }

        public async Task<BaseResult<UpdateUserDto>> GetUserByIdAsync(Guid userId)
        {
            var user = await unitOfWork.Users.GetAll()
                .Select(x => mapper.Map<UpdateUserDto>(x))
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null)
            {
                return BaseResult<UpdateUserDto>.Failure((int)ErrorCodes.UserNotFound, ErrorMessage.UserNotFound);
            }

            return BaseResult<UpdateUserDto>.Success(user);
        }

        public async Task<CollectionResult<UserDto>> GetUsersAsync()
        {
            var users = await unitOfWork.Users.GetAll()
                .Select(x => mapper.Map<UserDto>(x))
                .ToArrayAsync();

            return CollectionResult<UserDto>.Success(users);
        }

        public async Task<BaseResult<UpdateUserDto>> UpdateUserDataAsync(UpdateUserDto dto)
        {
            var user = await unitOfWork.Users.GetAll()
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (user is null)
            {
                return BaseResult<UpdateUserDto>.Failure((int)ErrorCodes.UserNotFound, ErrorMessage.UserNotFound);
            }

            user = mapper.Map<User>(dto);

            unitOfWork.Users.Update(user);
            await unitOfWork.SaveChangesAsync();

            return BaseResult<UpdateUserDto>.Success(dto);
        }
    }
}
