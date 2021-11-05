using DataLayer.EfCode;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceLayer.DatabaseServices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookApp.HelperExtensions
{
    public static class DatabaseStartupHelpers
    {
        public static async Task<IHost> SetupDatabaseAsync(this IHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var env = services.GetRequiredService<IWebHostEnvironment>();
                var context = services.GetRequiredService<EfCoreContext>();

                try
                {
                    var arePendingMigrations = context.Database.GetPendingMigrations().Any();
                    
                    if (arePendingMigrations)
                    {
                        await context.Database.MigrateAsync();
                        
                    }
                    await context.SeedDatabaseIfNoBooksAsync(env.WebRootPath);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occured while creating/migrating or seeding the database.");
                    throw;
                }
            }
            return webHost;
        }
    }
}
