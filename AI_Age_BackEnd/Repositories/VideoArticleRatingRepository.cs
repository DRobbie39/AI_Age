using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AI_Age_BackEnd.Repositories
{
    public class VideoArticleRatingRepository : IVideoArticleRatingRepository
    {
        private readonly AI_AgeContext _context;

        public VideoArticleRatingRepository(AI_AgeContext context)
        {
            _context = context;
        }

        public async Task AddRatingAsync(VideoArticleRating videoArticleRating)
        {
            await _context.VideoArticleRatings.AddAsync(videoArticleRating);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetAverageRatingAsync(int videoId)
        {
            return await _context.VideoArticleRatings
                .Where(r => r.VideoId == videoId)
                .AverageAsync(r => (decimal?)r.RatingValue) ?? 0.0m;
        }

        public async Task<VideoArticleRating> GetUserRatingAsync(int videoId, int userId)
        {
            return await _context.VideoArticleRatings
                .FirstOrDefaultAsync(r => r.VideoId == videoId && r.UserId == userId);
        }
    }
}
