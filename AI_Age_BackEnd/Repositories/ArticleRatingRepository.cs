using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AI_Age_BackEnd.Repositories
{
    public class ArticleRatingRepository : IArticleRatingRepository
    {
        private readonly AI_AgeContext _context;

        public ArticleRatingRepository(AI_AgeContext context)
        {
            _context = context;
        }

        public async Task AddRatingAsync(ArticleRating articleRating)
        {
            await _context.ArticleRatings.AddAsync(articleRating);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetAverageRatingAsync(int articleId)
        {
            return await _context.ArticleRatings
                .Where(r => r.ArticleId == articleId)
                .AverageAsync(r => (decimal?)r.RatingValue) ?? 0.0m;
        }
    }
}
