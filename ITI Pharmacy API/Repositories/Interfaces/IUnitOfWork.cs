namespace ITI.Pharmacy.API.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Repository properties
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        ICartRepository Carts { get; }
        IOrderRepository Orders { get; }
        IWishlistRepository Wishlists { get; }
        IPharmacyBranchRepository PharmacyBranches { get; }
        IProductStockRepository ProductStocks { get; }
        IUserRepository Users { get; }
        IReviewRepository Reviews { get; }

        // Generic repository access
        IGenericRepository<T> Repository<T>() where T : class;

        // Transaction methods
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
