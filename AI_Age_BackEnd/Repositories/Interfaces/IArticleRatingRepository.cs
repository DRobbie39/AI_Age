using AI_Age_BackEnd.Models;

namespace AI_Age_BackEnd.Repositories.Interfaces
{
    public interface IArticleRatingRepository
    {
        Task AddRatingAsync(ArticleRating articleRating); 
        Task<decimal> GetAverageRatingAsync(int articleId);
        Task<ArticleRating> GetUserRatingAsync(int articleId, int userId);
    }
}
