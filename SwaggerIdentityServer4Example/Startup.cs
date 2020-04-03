using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace SwaggerIdentityServer4Example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .Build()));
            });

            services.AddControllers();

            ConfigureAuth(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://localhost:44333/connect/authorize"),
                            TokenUrl = new Uri("https://localhost:44333/connect/token"),
                            Scopes = new Dictionary<string, string>()
                            {
                                {
                                    "demo-test-api-scope", "Demo API Swagger Auth Example"
                                }
                            },
                        }
                    }
                });
                c.OperationFilter<AuthorizeCheckOperationFilter>();
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

                c.OAuthClientId("mvc-test-api-swagger-app");
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        protected void ConfigureAuth(IServiceCollection services)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:44333";  // identity server endpoint
                    options.ApiName = "demo-test-api-scope";
                    options.RequireHttpsMetadata = false; // this should be true on production
                });
        }
    }
}
