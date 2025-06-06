using BusinessObjects.Models;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CosmeticInformationRepository : ICosmeticInformationRepository
    {
        public async Task<CosmeticInformation> Add(CosmeticInformation cosmeticInformation)
        {
            return await CosmeticInformationDAO.Instance.AddCosmeticInformation(cosmeticInformation);
        }

        public async Task<CosmeticInformation> Delete(string id)
        {
            return await CosmeticInformationDAO.Instance.Delete(id);
        }

        public async Task<List<CosmeticCategory>> GetAllCategories()
        {
            return await CosmeticInformationDAO.Instance.GetAllCategories();
        }

        public async Task<List<CosmeticInformation>> GetAllCosmetics()
        {
            return await CosmeticInformationDAO.Instance.GetAllCosmetics();
        }

        public async Task<CosmeticInformation> GetOne(string id)
        {
            return await CosmeticInformationDAO.Instance.GetById(id);
        }

        public async Task<CosmeticInformation> Update(CosmeticInformation cosmeticInformation)
        {
            return await CosmeticInformationDAO.Instance.Update(cosmeticInformation);
        }
    }
}