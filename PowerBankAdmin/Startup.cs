using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PowerBankAdmin.Data.Interfaces;
using PowerBankAdmin.Data.Repository;
using PowerBankAdmin.Services;

namespace PowerBankAdmin
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISmsService, Services.SMSService>();
            services.AddTransient<IHolderService, Services.HolderService>();
            services.AddTransient<IGeocodeService, Services.GeocodeService>();

            var connection = Configuration.GetConnectionString("ProdConnection");
            //services.AddDbContext<AppRepository>(options => options.UseInMemoryDatabase("local"));
            services.AddDbContext<AppRepository>(options => options.UseSqlServer(connection));
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            /* if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }*/

            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            app.UseMiddleware<CostumerAuthorizationMiddleware>();
            app.UseMiddleware<AuthorizationMiddleware>();
            app.UseMvc();
            
        }
    }
}
