using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AI_Age_BackEnd.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly AI_AgeContext _context;

        public RatingRepository(AI_AgeContext context)
        {
            _context = context;
        }

        public async Task AddRatingAsync(Rating rating)
        {
            await _context.Ratings.AddAsync(rating);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetAverageRatingAsync(int articleId)
        {
            return await _context.Ratings
                .Where(r => r.ArticleId == articleId)
                .AverageAsync(r => (decimal?)r.RatingValue) ?? 0.0m;
        }
    }
}
