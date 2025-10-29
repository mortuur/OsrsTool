using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OsrsTracker.Domain.Entities;


namespace OsrsTool.Infrastructure.Data
{
    public class GenericDbContextFactory : IDesignTimeDbContextFactory<GenericDbContext<Item>>
    {
        public GenericDbContext<Item> CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GenericDbContext<Item>>();
            optionsBuilder.UseSqlite("Data Source=osrs.db");
            return new GenericDbContext<Item>(optionsBuilder.Options);
        }
    }
}
