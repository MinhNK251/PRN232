using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class SystemAccountDAO
    {
        private static SystemAccountDAO? instance = null;

        private SystemAccountDAO() { }

        public static SystemAccountDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SystemAccountDAO();
                }
                return instance;
            }
        }
        public async Task<SystemAccount> Login(string email, string password)
        {
            using (var context = new CosmeticsDbContext())
            {
                var account = await context.SystemAccounts.FirstOrDefaultAsync(account => account.EmailAddress == email && account.AccountPassword == password);
                return account;
            }
        }
    }
}
