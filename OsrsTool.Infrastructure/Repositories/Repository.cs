using Microsoft.EntityFrameworkCore;
using OsrsTool.Domain.Interfaces;
using OsrsTool.Infrastructure.Data;
using System.Linq.Expressions;

namespace OsrsTool.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly GenericDbContext<T> _context;
        private readonly DbSet<T> _dbSet;

        public Repository(GenericDbContext<T> context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Remove(T entity) => _dbSet.Remove(entity);

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        Task<IEnumerable<T>> IRepository<T>.FindAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
