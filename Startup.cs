
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace Dentsu.Aegis.Api
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
          
            services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme).AddAzureADBearer(options => Configuration.Bind("AzureAD", options));

            // By default, the claims mapping will map claim names in the old format to accommodate older SAML applications.
            // 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role' instead of 'roles'
            // This flag ensures that the ClaimsIdentity claims collection will be built from the claims in the token
            JwtSecurityTokenHandler.DefaultMapInboundClaims = true;

            services.AddCors();

            services.AddControllers();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.0#middleware-order
            app.UseHttpsRedirection();          
            app.UseRouting();
            app.UseCors(options =>
            {
                options.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                        
            });
            app.UseAuthentication();            
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }
    }
}
