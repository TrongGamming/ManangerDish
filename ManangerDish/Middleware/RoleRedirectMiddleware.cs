using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ManagerDish.Middleware
{
    public class RoleRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            var authorizeAttribute = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>();
            if (authorizeAttribute == null)
            {
                await _next(context);
                return;
            }

            var user = context.User;

            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                context.Response.Redirect("/Home/Index");
                return;
            }

            if (!string.IsNullOrEmpty(authorizeAttribute.Roles))
            {
                var allowedRoles = authorizeAttribute.Roles.Split(',').Select(r => r.Trim());

                var userHasValidRole = allowedRoles.Any(role => user.IsInRole(role));
                if (!userHasValidRole)
                {
                    context.Response.Redirect("/Home/Index");
                    return;
                }
            }

            await _next(context);
        }
    }

}
