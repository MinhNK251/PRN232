using BusinessObjects.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepository _repo;
        public SystemAccountService(ISystemAccountRepository repo)
        {
            _repo = repo;
        }
        public async Task<SystemAccount> Login(string email, string password)
        {
            return await _repo.Login(email, password);
        }
    }
}
