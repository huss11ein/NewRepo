using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using TeleperformanceTask.Data;

namespace TeleperformanceTask.Extensions
{
    public static class MigrationExtenstion
    {
        public static void ApplyMigrations(this IApplicationBuilder app) {
            using IServiceScope scope  = app.ApplicationServices.CreateScope();
            using ApplicationDbContext dbcontext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbcontext.Database.Migrate();
        
        }
    }
}
