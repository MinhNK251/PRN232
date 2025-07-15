using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly Summer2025HandbagDbContext _context;
        public AccountRepository(Summer2025HandbagDbContext context)
        {
            _context = context;
        }
        public async Task<SystemAccount> GetByEmailPasswordAsync(string email, string password)
        {
            return await _context.SystemAccounts
                .FirstOrDefaultAsync(a => a.Email == email && a.Password == password && a.IsActive == true);
        }
    }
}
