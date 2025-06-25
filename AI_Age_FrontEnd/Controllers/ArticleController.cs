using AI_Age_FrontEnd.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AI_Age_FrontEnd.Controllers
{
    public class ArticleController : Controller
    {
        private readonly HttpClient _httpClient;

        public ArticleController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7022/api/");
        }

        public async Task<IActionResult> Index(string? query)
        {
            // Lưu lại query để hiển thị trên view
            ViewData["CurrentFilter"] = query;

            // Tạo đường dẫn API
            var requestUri = "Article/getallarticles";

            // Nếu có query, thêm vào URL
            if (!string.IsNullOrEmpty(query))
            {
                // Uri.EscapeDataString để mã hóa các ký tự đặc biệt trong query (ví dụ: dấu cách)
                requestUri += $"?query={Uri.EscapeDataString(query)}";
            }

            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            var articles = JsonSerializer.Deserialize<List<ArticleDto>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(articles);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"Article/{id}");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            var article = JsonSerializer.Deserialize<ArticleDto>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(article);
        }
    }
}
