using ITI.Pharmacy.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ITI.Pharmacy.Infrastructure.Data;

namespace ITI.Pharmacy.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return Task.FromResult(_dbSet.Where(predicate).AsEnumerable());
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }

        public virtual void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
