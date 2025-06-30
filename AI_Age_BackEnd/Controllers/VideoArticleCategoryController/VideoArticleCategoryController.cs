using AI_Age_BackEnd.DTOs.VideoArticleCategoryDto;
using AI_Age_BackEnd.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AI_Age_BackEnd.Controllers.VideoArticleCategoryController
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoArticleCategoryController : Controller
    {
        private readonly IVideoArticleCategoryRepository _categoryRepository;

        public VideoArticleCategoryController(IVideoArticleCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("getallcategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            var categoryDtos = categories.Select(c => new VideoArticleCategoryDto
            {
                Id = c.CategoryId,
                Name = c.CategoryName
            });
            return Ok(categoryDtos);
        }
    }
}
