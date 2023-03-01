using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using BikeStore.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeStoreWebApi.Extensions
{
    public static class EnvironmentVariablesInitializer
    {
        public static void SetEnvironmentVariablesFromKeyVault(this IServiceCollection services, IConfiguration configuration)
        {
            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                 }
            };

            var keyVaultURI = configuration["KeyVaultURI"];

            var client = new SecretClient(new Uri(keyVaultURI), new DefaultAzureCredential(), options);

            KeyVaultSecret secret = client.GetSecret("SqlConnectionString");

            configuration.GetSection("ConnectionStrings")["BikeStoreDB"] = secret.Value;
        }
    }
}
