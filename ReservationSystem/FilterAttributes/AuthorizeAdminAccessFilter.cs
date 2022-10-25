using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReservationSystem.Application.Extensions;
using ReservationSystem.Application.Services.Interfaces;

namespace ReservationSystem.Web.FilterAttributes
{
    public class AuthorizeAdminAccessFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userService = (IUserService)context.HttpContext.RequestServices.GetService(typeof(IUserService))!;

            var isUserAdmin = userService.IsUserAdmin(context.HttpContext.User.GetUserId()).Result;

            if (!isUserAdmin)
            {
                context.Result = new RedirectResult("/");
            }
        }
    }
}
