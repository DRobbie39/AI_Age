using AI_Age_FrontEnd.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
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

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("Article/getallarticles");
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
