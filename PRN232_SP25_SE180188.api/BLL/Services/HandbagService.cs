using BLL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class HandbagService : IHandbagService
    {
        private readonly IHandbagRepository _handbagRepository;

        public HandbagService(IHandbagRepository handbagRepository)
        {
            _handbagRepository = handbagRepository;
        }

        public async Task<List<Handbag>> GetAllAsync() => await _handbagRepository.GetAllAsync();

        public async Task<Handbag> GetByIdAsync(int id) => await _handbagRepository.GetByIdAsync(id);

        public async Task<List<Handbag>> SearchAsync(string modelName, string material) => await _handbagRepository.SearchAsync(modelName, material);

        public async Task<(bool IsSuccess, string ErrorCode, string ErrorMessage)> AddAsync(Handbag handbag)
        {
            if (string.IsNullOrWhiteSpace(handbag.ModelName) ||
                !System.Text.RegularExpressions.Regex.IsMatch(handbag.ModelName, @"^([A-Z0-9][a-zA-Z0-9#]*\\s)*([A-Z0-9][a-zA-Z0-9#]*)$"))
                return (false, "HB40001", "modelName is required or invalid");

            if (handbag.Price <= 0 || handbag.Stock <= 0)
                return (false, "HB40001", "price and stock must be greater than 0");

            await _handbagRepository.AddAsync(handbag);
            return (true, null, null);
        }

        public async Task<(bool IsSuccess, string ErrorCode, string ErrorMessage)> UpdateAsync(Handbag handbag)
        {
            var existing = await _handbagRepository.GetByIdAsync(handbag.HandbagId);
            if (existing == null)
                return (false, "HB40401", "Handbag not found");

            if (string.IsNullOrWhiteSpace(handbag.ModelName) ||
                !System.Text.RegularExpressions.Regex.IsMatch(handbag.ModelName, @"^([A-Z0-9][a-zA-Z0-9#]*\\s)*([A-Z0-9][a-zA-Z0-9#]*)$"))
                return (false, "HB40001", "modelName is required or invalid");

            if (handbag.Price <= 0 || handbag.Stock <= 0)
                return (false, "HB40001", "price and stock must be greater than 0");

            await _handbagRepository.UpdateAsync(handbag);
            return (true, null, null);
        }

        public async Task<(bool IsSuccess, string ErrorCode, string ErrorMessage)> DeleteAsync(int id)
        {
            var existing = await _handbagRepository.GetByIdAsync(id);
            if (existing == null)
                return (false, "HB40401", "Handbag not found");

            await _handbagRepository.DeleteAsync(id);
            return (true, null, null);
        }
    }
}
