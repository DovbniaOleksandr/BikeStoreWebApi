using BikeStore.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStoreWebApi.Extensions
{
    public static class DatabaseInjectionExtension
    {
        public static void ConfigureDatabase(this IServiceCollection services)
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME"); ;
            var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "sa";

            var connectionString = $"Data Source={host};Initial Catalog={dbName};User ID={dbUser};Password={dbPassword}";

            services.AddDbContext<BikeStoreDBContext>(options => options.UseSqlServer(connectionString, x => x.MigrationsAssembly("BikeStore.DAL")));
        }
    }
}
