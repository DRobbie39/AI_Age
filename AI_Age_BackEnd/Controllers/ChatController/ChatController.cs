using AI_Age_BackEnd.DTOs.ChatDTO;
using AI_Age_BackEnd.Services.ChatService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AI_Age_BackEnd.Controllers.ChatController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : Controller
    {
        private readonly ChatService _chatService;

        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [Authorize]
        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] ChatDto chatDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdString = User.FindFirstValue("UserId");

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized(new { Message = "Token không hợp lệ hoặc không chứa thông tin người dùng." });
            }

            try
            {
                var response = await _chatService.GetChatResponseAsync(chatDto, userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new AIResponseDto
                {
                    Action = "talk",
                    Response = "Có lỗi xảy ra. Vui lòng thử lại.",
                    Url = null
                });
            }
        }

        [Authorize]
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var userIdString = User.FindFirstValue("UserId") ?? User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized(new { Message = "Token không hợp lệ." });
            }

            try
            {
                var history = await _chatService.GetChatHistoryAsync(userId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi máy chủ khi lấy lịch sử trò chuyện." });
            }
        }
    }
}
