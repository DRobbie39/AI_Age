using AI_Age_BackEnd.DTOs.ArticleDTO;
using AI_Age_BackEnd.Services.ArticleService;
using Microsoft.AspNetCore.Mvc;

namespace AI_Age_BackEnd.Controllers.ArticleController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : Controller
    {
        private readonly ArticleService _articleService;

        public ArticleController(ArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet("getallarticles")]
        public async Task<IActionResult> GetAllArticles()
        {
            try
            {
                var articles = await _articleService.GetAllArticlesAsync();
                return Ok(articles);
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Không thể lấy danh sách bài viết. Vui lòng thử lại." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleById(int id)
        {
            try
            {
                var article = await _articleService.GetArticleByIdAsync(id);
                return Ok(article);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("createarticle")]
        public async Task<IActionResult> CreateArticle([FromForm] ArticleCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var article = await _articleService.CreateArticleAsync(dto);
                return Ok(new { Message = "Tạo bài viết thành công", Article = article });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("updatearticle")]
        public async Task<IActionResult> UpdateArticle([FromForm] ArticleUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var article = await _articleService.UpdateArticleAsync(dto);
                return Ok(new { Message = "Cập nhật bài viết thành công", Article = article });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            try
            {
                await _articleService.DeleteArticleAsync(id);
                return Ok(new { Message = "Xóa bài viết thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
