using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AI_Age_BackEnd.Repositories
{
    public class ArticleImageRepository : IArticleImageRepository
    {
        private readonly AI_AgeContext _context;

        public ArticleImageRepository(AI_AgeContext context)
        {
            _context = context;
        }

        public async Task AddArticleImageAsync(ArticleImage image)
        {
            await _context.ArticleImages.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteArticleImagesByArticleIdAsync(int articleId)
        {
            var images = await _context.ArticleImages.Where(img => img.ArticleId == articleId).ToListAsync();
            _context.ArticleImages.RemoveRange(images);
            await _context.SaveChangesAsync();
        }
    }
}
