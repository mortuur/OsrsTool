using Microsoft.EntityFrameworkCore;
using OsrsTool.Domain.Entities;

namespace OsrsTool.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Item> Items => Set<Item>();
        public DbSet<ItemPriceHistory> ItemPriceHistory => Set<ItemPriceHistory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
