using AI_Age_BackEnd.DTOs.ArticleCategoryDTO;
using AI_Age_BackEnd.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AI_Age_BackEnd.Controllers.ArticleCategoryController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleCategoryController : Controller
    {
        private readonly IArticleCategoryRepository _categoryRepository;

        public ArticleCategoryController(IArticleCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("getallcategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            var categoryDtos = categories.Select(c => new ArticleCategoryDto
            {
                Id = c.CategoryId,
                Name = c.CategoryName
            });
            return Ok(categoryDtos);
        }
    }
}
