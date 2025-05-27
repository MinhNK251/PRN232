using BusinessObjects.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService()
        {
            _accountRepository = new AccountRepository();
        }
        public AccountMember GetAccountById(string accountId)
        {
            return _accountRepository.GetAccountById(accountId);
        }
    }
}
