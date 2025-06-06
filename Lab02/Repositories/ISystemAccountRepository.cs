using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ISystemAccountRepository
    {
        Task<SystemAccount> Login(string email, string password);
    }
}
