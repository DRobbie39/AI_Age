using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AI_Age_BackEnd.Repositories
{
    public class UserPostRepository : IUserPostRepository
    {
        private readonly AI_AgeContext _context;

        public UserPostRepository(AI_AgeContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserPost>> GetAllPostsAsync()
        {
            return await _context.UserPosts
                .Include(p => p.User)
                .Include(p => p.UserPostComments)
                    .ThenInclude(c => c.User)
                .OrderByDescending(p => p.PostedDate)
                .ToListAsync();
        }

        public async Task<UserPost?> GetPostByIdAsync(int id)
        {
            return await _context.UserPosts
                .Include(p => p.User)
                .Include(p => p.UserPostComments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(p => p.PostId == id);
        }

        public async Task<UserPost> AddPostAsync(UserPost post)
        {
            _context.UserPosts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task UpdatePostAsync(UserPost post)
        {
            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await _context.UserPosts.FindAsync(id);
            if (post != null)
            {
                _context.UserPosts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }
    }
}
