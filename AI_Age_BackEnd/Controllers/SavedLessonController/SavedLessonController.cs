using AI_Age_BackEnd.DTOs.SavedLessonTO;
using AI_Age_BackEnd.Services.SavedLessonService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AI_Age_BackEnd.Controllers.SavedLessonController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SavedLessonController : Controller
    {
        private readonly SavedLessonService _savedLessonService;

        public SavedLessonController(SavedLessonService savedLessonService)
        {
            _savedLessonService = savedLessonService;
        }

        private int GetCurrentUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                throw new UnauthorizedAccessException("UserID không hợp lệ hoặc không tồn tại trong token.");
            }
            return userId;
        }

        [HttpPost]
        public async Task<IActionResult> SaveLesson([FromBody] SavedLessonCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userId = GetCurrentUserId();
                var result = await _savedLessonService.SaveLessonAsync(userId, dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi hệ thống: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMySavedLessons()
        {
            try
            {
                var userId = GetCurrentUserId();
                var lessons = await _savedLessonService.GetMySavedLessonsAsync(userId);
                return Ok(lessons);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi hệ thống: " + ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSavedLesson(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _savedLessonService.DeleteSavedLessonAsync(userId, id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi hệ thống: " + ex.Message });
            }
        }

        [HttpGet("check")]
        public async Task<IActionResult> CheckSavedStatus([FromQuery] int? articleId, [FromQuery] int? videoId)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _savedLessonService.CheckIfLessonIsSavedAsync(userId, articleId, videoId);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                // Nếu người dùng chưa đăng nhập, trả về false mà không báo lỗi
                return Ok(new { isSaved = false });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi kiểm tra: " + ex.Message });
            }
        }
    }
}
