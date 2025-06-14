using AI_Age_BackEnd.Models;

namespace AI_Age_BackEnd.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        Task AddRatingAsync(Rating rating); 
        Task<decimal> GetAverageRatingAsync(int articleId);
    }
}
