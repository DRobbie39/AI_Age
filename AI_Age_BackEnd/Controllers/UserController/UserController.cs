using AI_Age_BackEnd.DTOs.UserDTO;
using AI_Age_BackEnd.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AI_Age_BackEnd.Controllers.UserController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        // Lấy UserID từ token
        private int GetCurrentUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdString, out var userId))
            {
                return userId;
            }

            throw new UnauthorizedAccessException("UserID không hợp lệ trong token.");
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = GetCurrentUserId();
            var userProfile = await _userService.GetUserProfileByIdAsync(userId);
            if (userProfile == null)
            {
                return NotFound(new { message = "Không tìm thấy người dùng." });
            }
            return Ok(userProfile);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UserUpdateDto userUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = GetCurrentUserId();
                var updatedProfile = await _userService.UpdateUserProfileAsync(userId, userUpdateDto);

                if (updatedProfile == null)
                {
                    return NotFound(new { message = "Không tìm thấy người dùng để cập nhật." });
                }

                return Ok(new { message = "Cập nhật thông tin thành công!", profile = updatedProfile });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi máy chủ: {ex.Message}" });
            }
        }
    }
}
