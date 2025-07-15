using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IHandbagRepository
    {
        Task<List<Handbag>> GetAllAsync();
        Task<Handbag> GetByIdAsync(int id);
        Task<List<Handbag>> SearchAsync(string modelName, string material);
        Task AddAsync(Handbag handbag);
        Task UpdateAsync(Handbag handbag);
        Task DeleteAsync(int id);
    }
}
