using AI_Age_FrontEnd.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AI_Age_FrontEnd.Controllers
{
    public class VideoArticleController : Controller
    {
        private readonly HttpClient _httpClient;

        public VideoArticleController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7022/api/");
        }

        public async Task<IActionResult> Index(string? query)
        {
            ViewData["CurrentFilter"] = query;

            var requestUri = "VideoArticle/getallvideoarticles";

            if (!string.IsNullOrEmpty(query))
            {
                // Uri.EscapeDataString để mã hóa các ký tự đặc biệt trong query (ví dụ: dấu cách)
                requestUri += $"?query={Uri.EscapeDataString(query)}";
            }

            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var videoArticles = JsonSerializer.Deserialize<List<VideoArticleDto>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(videoArticles);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"VideoArticle/{id}");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var videoArticle = JsonSerializer.Deserialize<VideoArticleDto>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(videoArticle);
        }
    }
}
