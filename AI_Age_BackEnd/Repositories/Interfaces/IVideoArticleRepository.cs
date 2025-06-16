using AI_Age_BackEnd.Models;

namespace AI_Age_BackEnd.Repositories.Interfaces
{
    public interface IVideoArticleRepository
    {
        Task<VideoArticle> GetVideoArticleByIdAsync(int id);
        Task<List<VideoArticle>> GetAllVideoArticlesAsync();
        Task AddVideoArticleAsync(VideoArticle videoArticle);
        Task UpdateVideoArticleAsync(VideoArticle videoArticle);
        Task DeleteVideoArticleAsync(int id);
    }
}
