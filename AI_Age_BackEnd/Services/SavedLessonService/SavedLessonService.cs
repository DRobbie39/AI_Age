using AI_Age_BackEnd.DTOs.SavedLessonTO;
using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using System.Security.Claims;

namespace AI_Age_BackEnd.Services.SavedLessonService
{
    public class SavedLessonService
    {
        private readonly ISavedLessonRepository _savedLessonRepository;

        public SavedLessonService(ISavedLessonRepository savedLessonRepository)
        {
            _savedLessonRepository = savedLessonRepository;
        }

        public async Task<SavedLessonDto> SaveLessonAsync(int userId, SavedLessonCreateDto dto)
        {
            if (!dto.ArticleId.HasValue && !dto.VideoId.HasValue)
            {
                throw new ArgumentException("Cần cung cấp ArticleId hoặc VideoId để lưu.");
            }
            if (dto.ArticleId.HasValue && dto.VideoId.HasValue)
            {
                throw new ArgumentException("Không thể lưu cả ArticleId và VideoId cùng lúc.");
            }

            var existing = await _savedLessonRepository.FindAsync(userId, dto.ArticleId, dto.VideoId);
            if (existing != null)
            {
                throw new InvalidOperationException("Bài học này đã được lưu trước đó.");
            }

            var savedLesson = new SavedLesson // Sửa lại tên model nếu bạn dùng scaffold
            {
                UserId = userId, // Sử dụng userId từ tham số
                ArticleId = dto.ArticleId,
                VideoId = dto.VideoId,
                LessonTitle = dto.LessonTitle,
                LessonImage = dto.LessonImage,
                LessonUrl = dto.LessonUrl,
                SavedDate = DateTime.Now
            };

            await _savedLessonRepository.AddAsync(savedLesson);

            return MapToDto(savedLesson);
        }

        public async Task<List<SavedLessonDto>> GetMySavedLessonsAsync(int userId)
        {
            var lessons = await _savedLessonRepository.GetByUserIdAsync(userId);

            return lessons.Select(MapToDto).ToList();
        }

        public async Task DeleteSavedLessonAsync(int userId, int savedLessonId)
        {
            var lessonToDelete = await _savedLessonRepository.GetByIdAsync(savedLessonId);

            if (lessonToDelete == null)
            {
                throw new KeyNotFoundException("Không tìm thấy bài học đã lưu.");
            }

            // Quan trọng: Kiểm tra quyền sở hữu bằng userId từ tham số
            if (lessonToDelete.UserId != userId)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền xóa bài học này.");
            }

            await _savedLessonRepository.DeleteAsync(lessonToDelete);
        }

        public async Task<object> CheckIfLessonIsSavedAsync(int userId, int? articleId, int? videoId)
        {
            var savedLesson = await _savedLessonRepository.FindAsync(userId, articleId, videoId);
            return new { isSaved = savedLesson != null, savedLessonId = savedLesson?.SavedLessonId };
        }

        private SavedLessonDto MapToDto(SavedLesson lesson) // Sửa lại tên model
        {
            return new SavedLessonDto
            {
                SavedLessonId = lesson.SavedLessonId,
                LessonType = lesson.ArticleId.HasValue ? "Article" : "Video",
                LessonId = lesson.ArticleId ?? lesson.VideoId ?? 0,
                LessonTitle = lesson.LessonTitle,
                LessonImage = lesson.LessonImage,
                LessonUrl = lesson.LessonUrl,
                SavedDate = lesson.SavedDate.GetValueOrDefault()
            };
        }
    }
}
