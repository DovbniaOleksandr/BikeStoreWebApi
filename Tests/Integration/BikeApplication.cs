using BikeStore.DAL;
using BikeStoreWebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Helpers;

namespace Tests.Integration
{
    public class BikeApplication: WebApplicationFactory<Program>
    {
        string inMemoryDatabaseConnectionString = Guid.NewGuid().ToString();

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                var configuration = config.Build();
            });

            builder.ConfigureServices(services => 
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BikeStoreDBContext>));
                services.Remove(descriptor);

                services.AddDbContext<BikeStoreDBContext>(options =>
                {
                    options.UseInMemoryDatabase(inMemoryDatabaseConnectionString);
                });

                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;

                    var context = scopedServices.GetRequiredService<BikeStoreDBContext>();

                    context.Database.EnsureCreated();

                    Utilities.InitDBForTest(context);
                }

                services.AddSingleton<IAuthorizationHandler, AllowAnonymous>();
            });

            return base.CreateHost(builder);
        }
    }

    public class AllowAnonymous : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            foreach (IAuthorizationRequirement requirement in context.PendingRequirements.ToList())
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
