using ITI.Pharmacy.Core.Entities;
using ITI.Pharmacy.Core.Interfaces.Repositories;
using ITI.Pharmacy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
