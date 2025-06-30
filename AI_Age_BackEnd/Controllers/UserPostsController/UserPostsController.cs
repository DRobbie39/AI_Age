using AI_Age_BackEnd.DTOs.PostDTO;
using AI_Age_BackEnd.Services.UserPostService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AI_Age_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPostsController : Controller
    {
        private readonly UserPostService _postService;

        public UserPostsController(UserPostService postService)
        {
            _postService = postService;
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


        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _postService.GetAllPostsAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            try
            {
                var post = await _postService.GetPostByIdAsync(id);
                return Ok(post);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromForm] UserPostCreateDto createDto)
        {
            try
            {
                var userId = GetCurrentUserId();

                var newPost = await _postService.CreatePostAsync(createDto, userId);
                return CreatedAtAction(nameof(GetPostById), new { id = newPost.PostID }, newPost);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePost(int id, [FromForm] UserPostUpdateDto updateDto)
        {
            try
            {
                var userId = GetCurrentUserId();

                var updatedPost = await _postService.UpdatePostAsync(id, updateDto, userId);
                return Ok(updatedPost);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
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

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                var userId = GetCurrentUserId();

                await _postService.DeletePostAsync(id, userId);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ nội bộ: " + ex.Message });
            }
        }
    }
}
