using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IAccountRepository
    {
        Task<SystemAccount> GetByEmailPasswordAsync(string email, string password);
    }
}
