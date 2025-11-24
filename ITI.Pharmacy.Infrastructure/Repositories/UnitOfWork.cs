using ITI.Pharmacy.Core.Interfaces.Repositories;
using ITI.Pharmacy.Infrastructure.Data;

namespace ITI.Pharmacy.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private ProductRepository? _productRepository;
        private OrderRepository? _orderRepository;
        private UserRepository? _userRepository;
        private CartRepository? _cartRepository;
        private WishlistRepository? _wishlistRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IProductRepository Products => _productRepository ??= new ProductRepository(_context);

        public IOrderRepository Orders => _orderRepository ??= new OrderRepository(_context);

        public IUserRepository Users => _userRepository ??= new UserRepository(_context);

        public ICartRepository Carts => _cartRepository ??= new CartRepository(_context);

        public IWishlistRepository Wishlists => _wishlistRepository ??= new WishlistRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
