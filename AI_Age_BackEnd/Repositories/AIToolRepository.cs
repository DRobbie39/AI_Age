using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AI_Age_BackEnd.Repositories
{
    public class AIToolRepository : IAIToolRepository
    {
        private readonly AI_AgeContext _context;

        public AIToolRepository(AI_AgeContext context)
        {
            _context = context;
        }

        public async Task<List<Aitool>> GetAllAsync(string? searchQuery = null, int? categoryId = null)
        {
            var query = _context.Aitools
                .Include(t => t.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(t => t.ToolName.Contains(searchQuery));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == categoryId.Value);
            }

            return await query
                .OrderBy(t => t.ToolName)
                .ToListAsync();
        }

        public async Task<Aitool?> GetByIdAsync(int id)
        {
            return await _context.Aitools
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.ToolId == id);
        }

        public async Task AddAsync(Aitool tool)
        {
            await _context.Aitools.AddAsync(tool);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Aitool tool)
        {
            _context.Aitools.Update(tool);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Aitool tool)
        {
            _context.Aitools.Remove(tool);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Aitool>> GetByCategoryIdAsync(int categoryId)
        {
            return await _context.Aitools
                .Where(t => t.CategoryId == categoryId)
                .OrderBy(t => t.ToolName)
                .ToListAsync();
        }
    }
}
