using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Settings;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Services.Services;


namespace Web.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            

            // requires using Microsoft.Extensions.Options
            services.Configure<CustomerDatabaseSettings>(
                Configuration.GetSection(nameof(CustomerDatabaseSettings)));

            services.AddSingleton<ICustomerDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<CustomerDatabaseSettings>>().Value);

            services.AddScoped<ICustomerService, CustomerService>();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            .AddIdentityServerAuthentication(options =>
              {
                  options.Authority = "https://demo.identityserver.io";
                  options.ApiName = "api";

              });
            services.AddControllers();

            //// services.AddSingleton<Customer>();
            ////  var client = new MongoClient("mongodb://localhost:27017");
            //var client = new MongoClient("mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass%20Community&ssl=false");
            //var database = client.GetDatabase("EpsilonNetTestDb");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            
           // app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
