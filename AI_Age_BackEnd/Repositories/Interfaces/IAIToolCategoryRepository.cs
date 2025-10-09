using AI_Age_BackEnd.Models;

namespace AI_Age_BackEnd.Repositories.Interfaces
{
    public interface IAIToolCategoryRepository
    {
        Task<List<AitoolCategory>> GetAllAsync(string? searchQuery = null);
        Task<AitoolCategory?> GetByIdAsync(int id);
        Task<AitoolCategory?> GetByNameAsync(string name);
        Task AddAsync(AitoolCategory category);
        Task UpdateAsync(AitoolCategory category);
        Task DeleteAsync(AitoolCategory category);
    }
}
