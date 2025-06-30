using AI_Age_BackEnd.Models;

namespace AI_Age_BackEnd.Repositories.Interfaces
{
    public interface IUserPostCommentRepository
    {
        Task<UserPostComment?> GetCommentByIdAsync(int id);
        Task<UserPostComment> AddCommentAsync(UserPostComment comment);
        Task UpdateCommentAsync(UserPostComment comment);
        Task DeleteCommentAsync(int id);
    }
}
