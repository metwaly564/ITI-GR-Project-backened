using ITI.Pharmacy.API.Models;
using ITI.Pharmacy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace ITI.Pharmacy.API.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        // Repositories
        private IProductRepository? _products;
        private ICategoryRepository? _categories;
        private ICartRepository? _carts;
        private IOrderRepository? _orders;
        private IWishlistRepository? _wishlists;
        private IPharmacyBranchRepository? _pharmacyBranches;
        private IProductStockRepository? _productStocks;
        private IUserRepository? _users;

        // Generic repositories cache
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IProductRepository Products =>
            _products ??= new ProductRepository(_context);

        public ICategoryRepository Categories =>
            _categories ??= new CategoryRepository(_context);

        public ICartRepository Carts =>
            _carts ??= new CartRepository(_context);

        public IOrderRepository Orders =>
            _orders ??= new OrderRepository(_context);

        public IWishlistRepository Wishlists =>
            _wishlists ??= new WishlistRepository(_context);

        public IPharmacyBranchRepository PharmacyBranches =>
            _pharmacyBranches ??= new PharmacyBranchRepository(_context);

        public IProductStockRepository ProductStocks =>
            _productStocks ??= new ProductStockRepository(_context);

        public IUserRepository Users =>
            _users ??= new UserRepository(_context);

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new GenericRepository<T>(_context);
            }
            return (IGenericRepository<T>)_repositories[type];
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
