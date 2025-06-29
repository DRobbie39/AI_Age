using AI_Age_BackEnd.DTOs.AIToolCategoryDTO;
using AI_Age_BackEnd.Services.AIToolCategoryService;
using AI_Age_BackEnd.Services.AIToolService;
using Microsoft.AspNetCore.Mvc;

namespace AI_Age_BackEnd.Controllers.AIToolCategoryController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIToolCategoryController : Controller
    {
        private readonly AIToolCategoryService _categoryService;

        public AIToolCategoryController(AIToolCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                return Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AIToolCategoryCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var newCategory = await _categoryService.CreateCategoryAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = newCategory.CategoryID }, newCategory);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi hệ thống: " + ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AIToolCategoryUpdateDto dto)
        {
            if (id != dto.CategoryID)
            {
                return BadRequest("ID không khớp.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedCategory = await _categoryService.UpdateCategoryAsync(dto);
                return Ok(updatedCategory);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi hệ thống: " + ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
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
    }
}
