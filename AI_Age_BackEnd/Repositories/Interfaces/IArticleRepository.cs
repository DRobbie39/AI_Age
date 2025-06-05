using AI_Age_BackEnd.Models;

namespace AI_Age_BackEnd.Repositories.Interfaces
{
    public interface IArticleRepository
    {
        Task<Article> GetArticleByIdAsync(int id);
        Task<List<Article>> GetAllArticlesAsync();
        Task AddArticleAsync(Article article);
        Task UpdateArticleAsync(Article article);
        Task DeleteArticleAsync(int id);
    }
}
