using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Models;

namespace PowerBankAdmin.Services
{
    public class CostumerAuthorizationMiddleware
    {
        private readonly List<string> _pagesWithoutAuthCheck;
        private readonly RequestDelegate _next;

        public CostumerAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
            _pagesWithoutAuthCheck = new List<string>
            {
                "/admin",
                "/admin/auth/login",
                "/index",
                "/",
                "/costumer/registration"
            };
        }

        public async Task Invoke(HttpContext httpContext)
        {
            void RedirectToCostumerLogin()
            {
                httpContext.Response.Redirect(Strings.UrlCostumerRegistrationPage);
            }

            if (_pagesWithoutAuthCheck.Contains(httpContext.Request.Path.ToString().ToLower()))
            {
                await _next(httpContext);
                return;
            }

            var authToken = httpContext.Request.Cookies[Strings.CookieCostumerAuthToken];
            if (authToken == null)
            {
                RedirectToCostumerLogin();
                return;
            }

            var db = httpContext.RequestServices.GetService(typeof(AppRepository)) as AppRepository;
            var authorization = await db.CostumerAuthorizations.Include(a => a.Costumer).FirstOrDefaultAsync(x => x.AuthToken == authToken);

            if (authorization == null || authorization.Costumer == null)
            {
                RedirectToCostumerLogin();
                return;
            }
            var userToSendInHeader = authorization.Costumer;
            userToSendInHeader.Authorizations = new List<CostumerAuthorizationModel>();
            httpContext.Request.Headers.Add(Strings.CostumerObject, JsonConvert.SerializeObject(userToSendInHeader));

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CostumerAuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddlewareClassTemplate(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizationMiddleware>();
        }
    }
}

