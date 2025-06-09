using AI_Age_BackEnd.DTOs.ChatDTO;
using AI_Age_BackEnd.Services.ChatService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            try
            {
                var response = await _chatService.GetChatResponseAsync(chatDto);
                return Ok(new { message = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Có lỗi xảy ra. Vui lòng thử lại." });
            }
        }
    }
}
