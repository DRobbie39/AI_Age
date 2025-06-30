using AI_Age_BackEnd.DTOs.UserPostCommentDTO;
using AI_Age_BackEnd.Services.UserPostCommentService;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AI_Age_BackEnd.Controllers.UserPostCommentsController
{
    [Route("api/comments")]
    [ApiController]
    public class UserPostCommentsController : Controller
    {
        private readonly UserPostCommentService _commentService;

        public UserPostCommentsController(UserPostCommentService commentService)
        {
            _commentService = commentService;
        }

        private int GetCurrentUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdString, out var userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("UserID không hợp lệ hoặc không tìm thấy trong token.");
        }

        // POST: api/posts/{postId}/comments - Route này sẽ đẹp hơn
        [HttpPost("~/api/posts/{postId}/comments")]
        public async Task<IActionResult> CreateComment(int postId, [FromBody] UserPostCommentCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userId = GetCurrentUserId();
                var newComment = await _commentService.CreateCommentAsync(postId, createDto, userId);
                return CreatedAtAction(nameof(CreateComment), new { id = newComment.CommentID }, newComment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/comments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] UserPostCommentUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userId = GetCurrentUserId();
                var updatedComment = await _commentService.UpdateCommentAsync(id, updateDto, userId);
                return Ok(updatedComment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/comments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _commentService.DeleteCommentAsync(id, userId);
                return NoContent(); // 204 No Content là response chuẩn cho việc xóa thành công
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ nội bộ: " + ex.Message });
            }
        }
    }
}
