using AI_Age_BackEnd.Models;

namespace AI_Age_BackEnd.Repositories.Interfaces
{
    public interface IVideoArticleRatingRepository
    {
        Task AddRatingAsync(VideoArticleRating videoArticleRating);
        Task<decimal> GetAverageRatingAsync(int videoId);
        Task<VideoArticleRating> GetUserRatingAsync(int videoId, int userId);
    }
}
