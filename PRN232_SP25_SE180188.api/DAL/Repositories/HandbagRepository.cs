using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class HandbagRepository : IHandbagRepository
    {
        private readonly Summer2025HandbagDbContext _context;
        public HandbagRepository(Summer2025HandbagDbContext context)
        {
            _context = context;
        }

        public async Task<List<Handbag>> GetAllAsync()
        {
            return await _context.Handbags.Include(h => h.Brand).ToListAsync();
        }

        public async Task<Handbag> GetByIdAsync(int id)
        {
            return await _context.Handbags.Include(h => h.Brand).FirstOrDefaultAsync(h => h.HandbagId == id);
        }

        public async Task<List<Handbag>> SearchAsync(string modelName, string material)
        {
            return await _context.Handbags.Include(h => h.Brand)
                .Where(h => h.ModelName.Contains(modelName) && h.Material.Contains(material))
                .ToListAsync();
        }

        public async Task AddAsync(Handbag handbag)
        {
            _context.Handbags.Add(handbag);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Handbag handbag)
        {
            _context.Handbags.Update(handbag);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var handbag = await _context.Handbags.FindAsync(id);
            if (handbag != null)
            {
                _context.Handbags.Remove(handbag);
                await _context.SaveChangesAsync();
            }
        }
    }
}
