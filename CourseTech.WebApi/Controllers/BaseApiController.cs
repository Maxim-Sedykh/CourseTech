using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseTech.WebApi.Controllers
{
    public class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Идентификатор авторизованного пользователя
        /// </summary>
        protected Guid UserId
        {
            get
            { // To Do пофикси эксэпшн здесь
                return new Guid(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            }
        }
    }
}
