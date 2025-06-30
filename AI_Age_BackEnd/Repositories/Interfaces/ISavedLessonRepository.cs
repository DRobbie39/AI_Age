using AI_Age_BackEnd.Models;

namespace AI_Age_BackEnd.Repositories.Interfaces
{
    public interface ISavedLessonRepository
    {
        Task<SavedLesson?> FindAsync(int userId, int? articleId, int? videoId);
        Task<List<SavedLesson>> GetByUserIdAsync(int userId);
        Task AddAsync(SavedLesson savedLesson);
        Task DeleteAsync(SavedLesson savedLesson);
        Task<SavedLesson?> GetByIdAsync(int savedLessonId);
    }
}
