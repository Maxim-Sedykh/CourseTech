using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CourseTech.WebApi.Attributes
{
    public class AllowRolesAttribute : AuthorizationFilterAttribute
    {
        private readonly HashSet<string> _roles;

        public AllowRolesAttribute(params string[] roles)
        {
            _roles = new HashSet<string>(roles);
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var user = actionContext.RequestContext.Principal;

            if (user?.Identity.IsAuthenticated != true)
            {
                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                return;
            }

            if (!_roles.Any(user.IsInRole))
            {
                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Forbidden);
            }
        }
    }
}
