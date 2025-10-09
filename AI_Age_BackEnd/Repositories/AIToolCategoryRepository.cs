using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AI_Age_BackEnd.Repositories
{
    public class AIToolCategoryRepository : IAIToolCategoryRepository
    {
        private readonly AI_AgeContext _context;

        public AIToolCategoryRepository(AI_AgeContext context)
        {
            _context = context;
        }

        public async Task<List<AitoolCategory>> GetAllAsync(string? searchQuery = null)
        {
            var query = _context.AitoolCategories.AsQueryable();

            // Nếu có chuỗi tìm kiếm, thì thêm điều kiện Where
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(c => c.CategoryName.Contains(searchQuery));
            }

            return await query
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<AitoolCategory?> GetByIdAsync(int id)
        {
            return await _context.AitoolCategories.FindAsync(id);
        }

        public async Task<AitoolCategory?> GetByNameAsync(string name)
        {
            return await _context.AitoolCategories
                .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == name.ToLower());
        }

        public async Task AddAsync(AitoolCategory category)
        {
            await _context.AitoolCategories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AitoolCategory category)
        {
            _context.AitoolCategories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(AitoolCategory category)
        {
            _context.AitoolCategories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
