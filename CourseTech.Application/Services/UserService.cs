using AutoMapper;
using CourseTech.Application.Resources;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.Services
{
    public class UserService(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService) : IUserService
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

            var userToken = await unitOfWork.UserTokens.GetAll().FirstOrDefaultAsync(x => x.UserId == user.Id);

            using (var transaction = await unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    unitOfWork.Users.Remove(user);
                    unitOfWork.UserProfiles.Remove(userProfile);

                    if (userToken != null)
                    {
                        unitOfWork.UserTokens.Remove(userToken);
                    }

                    await unitOfWork.SaveChangesAsync();

                    await cacheService.RemoveAsync(CacheKeys.Users);

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
                .Include(x => x.UserProfile)
                .Include(x => x.Roles)
                .Where(x => x.Id == userId)
                .Select(x => mapper.Map<UpdateUserDto>(x))
                .FirstOrDefaultAsync();

            if (user is null)
            {
                return BaseResult<UpdateUserDto>.Failure((int)ErrorCodes.UserNotFound, ErrorMessage.UserNotFound);
            }

            return BaseResult<UpdateUserDto>.Success(user);
        }

        public async Task<CollectionResult<UserDto>> GetUsersAsync()
        {
            var users = await cacheService.GetOrAddToCache(
                CacheKeys.Users,
                async () => await unitOfWork.Users.GetAll()
                    .Include(x => x.Roles)
                    .Select(x => mapper.Map<UserDto>(x))
                    .ToArrayAsync());

            return CollectionResult<UserDto>.Success(users);
        }

        public async Task<BaseResult<UpdateUserDto>> UpdateUserDataAsync(UpdateUserDto dto)
        {
            var user = await unitOfWork.Users.GetAll()
                .Include(x => x.UserProfile)
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (user is null)
            {
                return BaseResult<UpdateUserDto>.Failure((int)ErrorCodes.UserNotFound, ErrorMessage.UserNotFound);
            }

            mapper.Map(dto, user);

            unitOfWork.Users.Update(user);
            await unitOfWork.SaveChangesAsync();

            await cacheService.RemoveAsync(CacheKeys.Users);

            return BaseResult<UpdateUserDto>.Success(dto);
        }
    }
}
