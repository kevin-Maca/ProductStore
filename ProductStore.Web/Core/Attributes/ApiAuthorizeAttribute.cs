using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProductStore.Web.Services.Abstractions;
using System.Security.Claims;

namespace ProductStore.Web.Core.Attributes
{
    public class ApiAuthorizeAttribute : TypeFilterAttribute
    {
        public ApiAuthorizeAttribute(string permission, string module) : base(typeof(ApiAuthorizeFilter))
        {
            Arguments = [module, permission];
        }
    }

    public class ApiAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly string _permission;
        private readonly string _module;
        private readonly IUsersServices _userService;

        public ApiAuthorizeFilter(string module, string permission, IUsersServices userService)
        {
            _module = module;
            _permission = permission;
            _userService = userService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            ClaimsPrincipal user = context.HttpContext.User;

            if (user?.Identity is null || !user.Identity.IsAuthenticated)
            {
                Response<object> response = Response<object>.Failure("No autenticado", ["El token no es válido"]);

                context.Result = new JsonResult(response)
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };

                return;
            }

            bool isAuthorized = await _userService.CurrentUserIsAuthorizedAsync(_permission, _module);

            if (!isAuthorized)
            {
                Response<object> response = Response<object>.Failure("Acceso denegado", ["No tienes permisos para realizar esta acción"]);
                context.Result = new JsonResult(response)
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
        }
    }
}
