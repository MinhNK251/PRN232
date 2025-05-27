using BusinessObjects.Models;
using DataAccessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class AccountDAO
    {
        public static AccountMember GetAccountById(string accountId)
        {
            using var context = new MyStoreDbContext();
            return context.AccountMembers.FirstOrDefault(a => a.MemberId.Equals(accountId));
        }        
    }
}
