using Microsoft.EntityFrameworkCore;

namespace OsrsTool.Infrastructure.Data
{
    public class GenericDbContext<T> : DbContext where T : class
    {
        public GenericDbContext(DbContextOptions<GenericDbContext<T>> options)
            : base(options) { }

        public DbSet<T> Entities => Set<T>();
    }
}
