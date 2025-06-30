using AI_Age_BackEnd.Models;

namespace AI_Age_BackEnd.Repositories.Interfaces
{
    public interface IUserPostRepository
    {
        Task<IEnumerable<UserPost>> GetAllPostsAsync();
        Task<UserPost?> GetPostByIdAsync(int id);
        Task<UserPost> AddPostAsync(UserPost post);
        Task UpdatePostAsync(UserPost post);
        Task DeletePostAsync(int id);
    }
}
