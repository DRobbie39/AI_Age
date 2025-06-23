using AI_Age_BackEnd.DTOs.VideoArticleDTO;
using AI_Age_BackEnd.DTOs.VideoArticleRatingDTO;
using AI_Age_BackEnd.Services.VideoArticleService;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AI_Age_BackEnd.Controllers.VideoArticleController
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoArticleController : Controller
    {
        private readonly VideoArticleService _videoArticleService;

        public VideoArticleController(VideoArticleService videoArticleService)
        {
            _videoArticleService = videoArticleService;
        }

        [HttpGet("getallvideoarticles")]
        public async Task<IActionResult> GetAllVideoArticles()
        {
            try
            {
                var videoArticles = await _videoArticleService.GetAllVideoArticlesAsync();
                return Ok(videoArticles);
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Không thể lấy danh sách video. Vui lòng thử lại." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVideoArticleById(int id)
        {
            try
            {
                var videoArticle = await _videoArticleService.GetVideoArticleByIdAsync(id);
                return Ok(videoArticle);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("createvideoarticle")]
        public async Task<IActionResult> CreateVideoArticle([FromForm] VideoArticleCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var videoArticle = await _videoArticleService.CreateVideoArticleAsync(dto);
                return Ok(new { Message = "Tạo video thành công", VideoArticle = videoArticle });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("updatevideoarticle")]
        public async Task<IActionResult> UpdateVideoArticle([FromForm] VideoArticleUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var videoArticle = await _videoArticleService.UpdateVideoArticleAsync(dto);
                return Ok(new { Message = "Cập nhật video thành công", VideoArticle = videoArticle });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideoArticle(int id)
        {
            try
            {
                await _videoArticleService.DeleteVideoArticleAsync(id);
                return Ok(new { Message = "Xóa video thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("rate")]
        public async Task<IActionResult> AddRating([FromBody] VideoArticleRatingCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _videoArticleService.AddRatingAsync(dto);
                var video = await _videoArticleService.GetVideoArticleByIdAsync(dto.VideoId);
                return Ok(new { Message = "Đánh giá video thành công", AverageRating = video.AverageRating });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("user-rating/{videoId}")]
        public async Task<IActionResult> GetUserRating(int videoId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var rating = await _videoArticleService.GetUserRatingAsync(videoId, userId);
                return Ok(new { RatingValue = rating });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
