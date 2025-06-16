using AI_Age_BackEnd.Models;

namespace AI_Age_BackEnd.Repositories.Interfaces
{
    public interface IVideoArticleCategoryRepository
    {
        Task<VideoArticleCategory> GetCategoryByIdAsync(int id);
    }
}
