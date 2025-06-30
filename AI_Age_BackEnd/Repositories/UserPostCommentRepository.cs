using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AI_Age_BackEnd.Repositories
{
    public class UserPostCommentRepository : IUserPostCommentRepository
    {
        private readonly AI_AgeContext _context;

        public UserPostCommentRepository(AI_AgeContext context)
        {
            _context = context;
        }

        public async Task<UserPostComment?> GetCommentByIdAsync(int id)
        {
            // Include User để có thể kiểm tra quyền sở hữu và lấy thông tin user
            return await _context.UserPostComments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CommentId == id);
        }

        public async Task<UserPostComment> AddCommentAsync(UserPostComment comment)
        {
            _context.UserPostComments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task UpdateCommentAsync(UserPostComment comment)
        {
            _context.Entry(comment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await _context.UserPostComments.FindAsync(id);
            if (comment != null)
            {
                _context.UserPostComments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
