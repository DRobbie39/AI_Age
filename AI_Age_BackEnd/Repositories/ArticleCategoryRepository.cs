using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;

namespace AI_Age_BackEnd.Repositories
{
    public class ArticleCategoryRepository : IArticleCategoryRepository
    {
        private readonly AI_AgeContext _context;

        public ArticleCategoryRepository(AI_AgeContext context)
        {
            _context = context;
        }

        public async Task<ArticleCategory> GetCategoryByIdAsync(int id)
        {
            return await _context.ArticleCategories.FindAsync(id);
        }
    }
}
