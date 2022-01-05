using BikeStore.Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStoreWebApi.Extensions
{
    public static class AuthenticationServiceExtension
    {
        public static void ConfigureAuthenticationService(this IServiceCollection services, IConfigurationSection authenticationConfig)
        {
            var authOtions = authenticationConfig.Get<AuthOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authOtions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = authOtions.Audience,

                        ValidateLifetime = true,

                        IssuerSigningKey = authOtions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                });

            services.Configure<AuthOptions>(authenticationConfig);
        }
    }
}
