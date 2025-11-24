using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;
using TekRemittance.Web.Models;

namespace TekRemittance.Web.Attributes
{
    public class RequirePermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _permission;

        public RequirePermissionAttribute(string permission)
        {
            _permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            
            if (!user.Identity?.IsAuthenticated == true)
            {
                context.Result = new UnauthorizedObjectResult(
                    ApiResponse<string>.Error("Authentication required", 401));
                return;
            }

            var userPermissions = user.FindAll("permission").Select(c => c.Value).ToList();
            
            if (!userPermissions.Contains(_permission))
            {
                context.Result = new ObjectResult(ApiResponse<string>.Error(
                    $"Access denied. Required permission: {_permission}", 403))
                {
                    StatusCode = 403
                };
                return;
            }
        }
    }
}
