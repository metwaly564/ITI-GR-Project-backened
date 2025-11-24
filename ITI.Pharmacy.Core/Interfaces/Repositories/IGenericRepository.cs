using System.Linq.Expressions;

namespace ITI.Pharmacy.Core.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        IQueryable<T> Query();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }
}
