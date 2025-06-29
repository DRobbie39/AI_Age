using AI_Age_BackEnd.DTOs.AIToolDTO;
using AI_Age_BackEnd.Services.AIToolService;
using Microsoft.AspNetCore.Mvc;

namespace AI_Age_BackEnd.Controllers.AIToolController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIToolController : Controller
    {
        private readonly AIToolService _toolService;

        public AIToolController(AIToolService toolService)
        {
            _toolService = toolService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tools = await _toolService.GetAllToolsAsync();
            return Ok(tools);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var tool = await _toolService.GetToolByIdAsync(id);
                return Ok(tool);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AIToolCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var newTool = await _toolService.CreateToolAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = newTool.ToolID }, newTool);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] AIToolUpdateDto dto)
        {
            if (id != dto.ToolID)
            {
                return BadRequest(new { Message = "ID của công cụ trong URL và trong nội dung request không khớp." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedTool = await _toolService.UpdateToolAsync(dto);
                return Ok(updatedTool);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _toolService.DeleteToolAsync(id);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi hệ thống: " + ex.Message });
            }
        }

        [HttpGet("ByCategory/{categoryId}")]
        public async Task<IActionResult> GetByCategoryId(int categoryId)
        {
            try
            {
                var tools = await _toolService.GetToolsByCategoryIdAsync(categoryId);
                return Ok(tools);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi lấy dữ liệu công cụ theo danh mục.");
            }
        }
    }
}
