using CourseTech.Domain.Dto.Role;
using CourseTech.Domain.Dto.UserRole;
using CourseTech.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис, предназначенный для управления ролей.
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// Получение всех ролей.
        /// </summary>
        /// <returns></returns>
        Task<CollectionResult<RoleDto>> GetAllRoles();

        /// <summary>
        /// Добавление роли.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<DataResult<RoleDto>> CreateRoleAsync(CreateRoleDto dto);

        /// <summary>
        /// Удаление роли по идентификатору.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DataResult<RoleDto>> DeleteRoleAsync(long id);

        /// <summary>
        /// Обновление роли.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult> UpdateRoleAsync(RoleDto dto);

        /// <summary>
        /// Добавление роли для пользователя.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<DataResult<UserRoleDto>> AddRoleForUserAsync(UserRoleDto dto);

        /// <summary>
        /// Удаление роли у пользователя.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<DataResult<UserRoleDto>> DeleteRoleForUserAsync(DeleteUserRoleDto dto);

        /// <summary>
        /// Обновление роли у пользователя.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<DataResult<UserRoleDto>> UpdateRoleForUserAsync(UpdateUserRoleDto dto);
    }
}
