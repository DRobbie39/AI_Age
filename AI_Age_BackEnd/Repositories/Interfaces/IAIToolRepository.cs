using AI_Age_BackEnd.Models;

namespace AI_Age_BackEnd.Repositories.Interfaces
{
    public interface IAIToolRepository
    {
        Task<List<Aitool>> GetAllAsync();
        Task<Aitool?> GetByIdAsync(int id);
        Task AddAsync(Aitool tool);
        Task UpdateAsync(Aitool tool);
        Task DeleteAsync(Aitool tool);
    }
}
