using System;

namespace ITI.Pharmacy.Core.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        IUserRepository Users { get; }
        ICartRepository Carts { get; }
        IWishlistRepository Wishlists { get; }
        Task<int> CommitAsync();
    }
}
