using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AI_Age_BackEnd.Repositories
{
    public class SavedLessonRepository : ISavedLessonRepository
    {
        private readonly AI_AgeContext _context;

        public SavedLessonRepository(AI_AgeContext context)
        {
            _context = context;
        }

        public async Task<SavedLesson?> FindAsync(int userId, int? articleId, int? videoId)
        {
            if (articleId.HasValue)
            {
                return await _context.SavedLessons
                    .FirstOrDefaultAsync(s => s.UserId == userId && s.ArticleId == articleId.Value);
            }
            if (videoId.HasValue)
            {
                return await _context.SavedLessons
                    .FirstOrDefaultAsync(s => s.UserId == userId && s.VideoId == videoId.Value);
            }
            return null;
        }

        public async Task<List<SavedLesson>> GetByUserIdAsync(int userId)
        {
            return await _context.SavedLessons
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.SavedDate)
                .ToListAsync();
        }

        public async Task AddAsync(SavedLesson savedLesson)
        {
            await _context.SavedLessons.AddAsync(savedLesson);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(SavedLesson savedLesson)
        {
            _context.SavedLessons.Remove(savedLesson);
            await _context.SaveChangesAsync();
        }

        public async Task<SavedLesson?> GetByIdAsync(int savedLessonId)
        {
            return await _context.SavedLessons.FindAsync(savedLessonId);
        }
    }
}
