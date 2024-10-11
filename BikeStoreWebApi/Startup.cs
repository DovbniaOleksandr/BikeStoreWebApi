using BikeStore.Core.Models;
using BikeStore.Core.Services;
using BikeStore.DAL;
using BikeStore.Services;
using BikeStoreEF;
using BikeStoreWebApi.Extensions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BikeStoreWebApi
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
            services.RegisterServices();

            services.AddControllers()
                .AddFluentValidation(s =>
                {
                    s.RegisterValidatorsFromAssemblyContaining<Startup>();
                    s.ImplicitlyValidateChildProperties = true;
                });

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Auth Demo Api enabled with JWT Bearer",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            services.AddDbContext<BikeStoreDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlConnectionString"), x => x.MigrationsAssembly("BikeStore.DAL")));
            services.AddAutoMapper(typeof(Startup));

            services.AddIdentity<User, Role>(options => 
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.User.RequireUniqueEmail = true;

                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                }).AddEntityFrameworkStores<BikeStoreDBContext>();

            services.ConfigureAuthenticationService(Configuration.GetSection("Auth"));

            services.AddHealthChecks();

            services.AddRateLimiter(options => {
                options.AddFixedWindowLimiter("Auth", opt => {
                    opt.Window = TimeSpan.FromSeconds(60);
                    opt.PermitLimit = 3;
                }).RejectionStatusCode = 429;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BikeStoreDBContext context, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                context.Database.EnsureCreated();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BikeStoreWebApi v1"));

            app.ConfigureExceptionHandler(logger);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseRateLimiter();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHealthChecks("/health");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
