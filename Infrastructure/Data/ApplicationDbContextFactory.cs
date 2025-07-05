using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Connection string — koristi onu iz appsettings.json
            optionsBuilder.UseSqlite("Data Source=../BlazorWeb/pushapps.db");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
