using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IHandbagService
    {
        Task<List<Handbag>> GetAllAsync();
        Task<Handbag> GetByIdAsync(int id);
        IQueryable<Handbag> Search(string? modelName, string? material);
        Task<(bool IsSuccess, string ErrorCode, string ErrorMessage)> AddAsync(Handbag handbag);
        Task<(bool IsSuccess, string ErrorCode, string ErrorMessage)> UpdateAsync(Handbag handbag);
        Task<(bool IsSuccess, string ErrorCode, string ErrorMessage)> DeleteAsync(int id);
    }
}
