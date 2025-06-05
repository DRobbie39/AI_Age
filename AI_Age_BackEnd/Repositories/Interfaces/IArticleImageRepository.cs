using AI_Age_BackEnd.Models;

namespace AI_Age_BackEnd.Repositories.Interfaces
{
    public interface IArticleImageRepository
    {
        Task AddArticleImageAsync(ArticleImage image);
        Task DeleteArticleImagesByArticleIdAsync(int articleId);
    }
}
