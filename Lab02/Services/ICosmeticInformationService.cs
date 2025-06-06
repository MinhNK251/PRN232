using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ICosmeticInformationService
    {
        Task<List<CosmeticInformation>> GetAllCosmetics();
        Task<CosmeticInformation> GetOne(string id);
        Task<CosmeticInformation> Add(CosmeticInformation cosmeticInformation);
        Task<CosmeticInformation> Update(CosmeticInformation cosmeticInformation);
        Task<CosmeticInformation> Delete(string id);
        Task<List<CosmeticCategory>> GetAllCategories();
    }
}
