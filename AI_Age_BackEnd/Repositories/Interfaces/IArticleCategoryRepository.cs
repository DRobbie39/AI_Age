using AI_Age_BackEnd.Models;

namespace AI_Age_BackEnd.Repositories.Interfaces
{
    public interface IArticleCategoryRepository
    {
        Task<ArticleCategory> GetCategoryByIdAsync(int id);
        Task<IEnumerable<ArticleCategory>> GetAllCategoriesAsync();
    }
}
