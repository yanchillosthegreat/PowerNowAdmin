using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using PowerBankAdmin.Data.Repository;

namespace PowerBankAdmin.Services
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            void RedirectToLogin() {
                httpContext.Response.Redirect("/Auth/Login");
            }

            if (httpContext.Request.Path.ToString().ToLower() == "/auth/login")
            {
                await _next(httpContext);
                return;
            }

            var authToken = httpContext.Request.Cookies["authToken"];
            if (authToken == null)
            {
                RedirectToLogin();
                return;
            }

            var db = httpContext.RequestServices.GetService(typeof(AppRepository)) as AppRepository;
            await db.Users.ToListAsync();
            var authorization = await db.Authorizations.FirstOrDefaultAsync(x => x.AuthToken == authToken);
            
            if (authorization == null || authorization.User == null)
            {
                RedirectToLogin();
                return;
            }
            var userToSendInHeader = authorization.User;
            userToSendInHeader.Authorizations = new List<AuthorizationModel>();
            httpContext.Request.Headers.Add("user", JsonConvert.SerializeObject(userToSendInHeader));

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddlewareClassTemplate(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizationMiddleware>();
        }
    }
}
