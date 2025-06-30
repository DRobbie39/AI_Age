using AI_Age_FrontEnd.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AI_Age_FrontEnd.Controllers
{
    public class VideoArticleController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public VideoArticleController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(string? query)
        {
            ViewData["CurrentFilter"] = query;

            var requestUri = "api/VideoArticle/getallvideoarticles";

            if (!string.IsNullOrEmpty(query))
            {
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
            var response = await _httpClient.GetAsync($"api/VideoArticle/{id}");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var videoArticle = JsonSerializer.Deserialize<VideoArticleDto>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Truyền API Base URL cho View để sử dụng trong JavaScript
            ViewBag.ApiBaseUrl = _configuration.GetValue<string>("ApiSettings:BaseUrl");

            return View(videoArticle);
        }
    }
}
