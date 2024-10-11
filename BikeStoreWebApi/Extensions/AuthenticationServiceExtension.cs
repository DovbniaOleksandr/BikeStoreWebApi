using BikeStore.Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authOtions.Secret)),
                    };
                });

            services.Configure<AuthOptions>(authenticationConfig);
        }
    }
}
