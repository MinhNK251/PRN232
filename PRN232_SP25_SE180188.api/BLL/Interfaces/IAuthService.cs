using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAuthService
    {
        Task<(SystemAccount Account, string ErrorCode, string ErrorMessage)> ValidateLoginAsync(string email, string password);
    }
}
