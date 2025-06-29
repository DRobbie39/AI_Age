using AI_Age_BackEnd.DTOs.ChatDTO;
using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AI_Age_BackEnd.Repositories
{
    public class SearchRepository : ISearchRepository
    {
        private readonly AI_AgeContext _context;

        public SearchRepository(AI_AgeContext context)
        {
            _context = context;
        }

        public async Task<List<SearchResultDto>> SearchContentAsync(string searchTerm)
        {
            var articles = await _context.Articles
                .Where(a => (a.Title.Contains(searchTerm) || a.Summary.Contains(searchTerm)))
                .Select(a => new SearchResultDto
                {
                    Type = "Article",
                    Id = a.ArticleId,
                    Title = a.Title,
                    Url = $"/Article/Details/{a.ArticleId}"
                })
                .ToListAsync();

            var videos = await _context.VideoArticles
                .Where(v => (v.Title.Contains(searchTerm) || v.Description.Contains(searchTerm)))
                .Select(v => new SearchResultDto
                {
                    Type = "Video",
                    Id = v.VideoId,
                    Title = v.Title,
                    Url = $"/VideoArticle/Details/{v.VideoId}"
                })
                .ToListAsync();

            return articles.Concat(videos).Take(5).ToList();
        }
    }
}
