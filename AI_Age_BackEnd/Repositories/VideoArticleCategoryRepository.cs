using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;

namespace AI_Age_BackEnd.Repositories
{
    public class VideoArticleCategoryRepository : IVideoArticleCategoryRepository
    {
        private readonly AI_AgeContext _context;

        public VideoArticleCategoryRepository(AI_AgeContext context)
        {
            _context = context;
        }

        public async Task<VideoArticleCategory> GetCategoryByIdAsync(int id)
        {
            return await _context.VideoArticleCategories.FindAsync(id);
        }
    }
}
