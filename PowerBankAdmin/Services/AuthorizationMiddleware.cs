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
using PowerBankAdmin.Models;

namespace PowerBankAdmin.Services
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthorizationMiddleware
    {
        private readonly List<string> _pagesWithoutAuthCheck;
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
            _pagesWithoutAuthCheck = new List<string>
            {
                "/admin/auth/login",
                "/index",
                "/",
                "/costumer/registration",
                "/costumer",
                "/take",
                "/tutorial",
                "/map",
                "/info/help",
                "/info/about",
                "/info/prices",
                "/info/cooperation",
                "/info/agreement"
            };
        }

        public async Task Invoke(HttpContext httpContext)
        {
            void RedirectToAdminLogin() {
                httpContext.Response.Redirect(Strings.UrlAdminLoginPage);
            }

            if (_pagesWithoutAuthCheck.Contains(httpContext.Request.Path.ToString().ToLower()))
            {
                await _next(httpContext);
                return;
            }

            if (httpContext.Request.Path.StartsWithSegments(new PathString("/acquiring")))
            {
                await _next(httpContext);
                return;
            }

            var authToken = httpContext.Request.Cookies[Strings.CookieAuthToken];
            if (authToken == null)
            {
                RedirectToAdminLogin();
                return;
            }

            var db = httpContext.RequestServices.GetService(typeof(AppRepository)) as AppRepository;
            var authorization = await db.Authorizations.Include(a => a.User).FirstOrDefaultAsync(x => x.AuthToken == authToken);
            
            if (authorization == null || authorization.User == null)
            {
                RedirectToAdminLogin();
                return;
            }
            var userToSendInHeader = (UserModel)authorization.User.Clone();
            userToSendInHeader.Authorizations = new List<AuthorizationModel>();
            httpContext.Request.Headers.Add(Strings.UserObject, JsonConvert.SerializeObject(userToSendInHeader));

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
