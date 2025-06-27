using AI_Age_BackEnd.DTOs.ChatDTO;

namespace AI_Age_BackEnd.Repositories.Interfaces
{
    public interface ISearchRepository
    {
        Task<List<SearchResultDto>> SearchContentAsync(string searchTerm);
    }
}
