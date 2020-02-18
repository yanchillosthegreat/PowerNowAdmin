using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly List<string> _restrictedPages;
        private readonly RequestDelegate _next;

        public CostumerAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
            _restrictedPages = new List<string> {
                "/take",
                "/costumer/index",
                "/costumer/addcard"
            }; //add here pages that shouldn't work without auth
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var authToken = httpContext.Request.Cookies[Strings.CookieCostumerAuthToken];
            var costumer = await GetCostumer(authToken, httpContext);
            //no permisions
            if (costumer == null && _restrictedPages.Contains(httpContext.Request.Path.ToString().ToLower()))
            {
                httpContext.Response.Redirect(Strings.UrlCostumerRegistrationPage);
                return;
            }

            if(costumer != null)
            {
                var costumerToSendInHeader = (CostumerModel)costumer.Clone();
                costumerToSendInHeader.Authorizations = new List<CostumerAuthorizationModel>();
                httpContext.Request.Headers.Add(Strings.CostumerObject, JsonConvert.SerializeObject(costumerToSendInHeader));

                var activeSession = costumer.Sessions.FirstOrDefault(x => x.IsActive);
                if (activeSession != null && httpContext.Request.Path == "/")
                {
                    httpContext.Response.Redirect($"/Take/SelectTariff/{activeSession.Powerbank.Holder.Id}");
                }
            }
            await _next(httpContext);
        }


        private async Task<CostumerModel> GetCostumer(string authToken, HttpContext httpContext)
        {
            if (string.IsNullOrEmpty(authToken))
            {
                return null;
            }

            var db = httpContext.RequestServices.GetService(typeof(AppRepository)) as AppRepository;
            var authorization = await db.CostumerAuthorizations.Include(a => a.Costumer).ThenInclude(x => x.CardBindings).Include(x => x.Costumer).ThenInclude(x => x.Sessions).ThenInclude(x => x.Powerbank).ThenInclude(x => x.Holder).FirstOrDefaultAsync(x => x.AuthToken == authToken);

            return authorization?.Costumer;
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

