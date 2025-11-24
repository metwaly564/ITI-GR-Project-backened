using ITI.Pharmacy.Core.Entities;

namespace ITI.Pharmacy.Core.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
