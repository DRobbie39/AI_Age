using AI_Age_FrontEnd.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AI_Age_FrontEnd.Controllers
{
    public class ArticleController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ArticleController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(string? query)
        {
            ViewData["CurrentFilter"] = query;

            var requestUri = "api/Article/getallarticles";

            if (!string.IsNullOrEmpty(query))
            {
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
            var response = await _httpClient.GetAsync($"api/Article/{id}");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            var article = JsonSerializer.Deserialize<ArticleDto>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Truyền API Base URL cho View để sử dụng trong JavaScript
            ViewBag.ApiBaseUrl = _configuration.GetValue<string>("ApiSettings:BaseUrl");

            return View(article);
        }
    }
}
