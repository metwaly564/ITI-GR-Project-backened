using ITI.Pharmacy.API.Models;
using ITI.Pharmacy.API.Repositories.Interfaces;

namespace ITI.Pharmacy.API.Repositories.Implementations
{
    public class PharmacyBranchRepository : GenericRepository<PharmacyBranch>, IPharmacyBranchRepository
    {
        public PharmacyBranchRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
