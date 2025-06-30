using AI_Age_FrontEnd.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Web;

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

        public async Task<IActionResult> Index(string? query, int? categoryId)
        {
            try
            {
                var categoryResponse = await _httpClient.GetAsync("api/VideoArticleCategory/getallcategories");
                categoryResponse.EnsureSuccessStatusCode();
                var categoryJson = await categoryResponse.Content.ReadAsStringAsync();

                ViewBag.Categories = JsonSerializer.Deserialize<List<VideoArticleCategoryDto>>(categoryJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                ViewBag.Categories = new List<VideoArticleCategoryDto>();
            }

            ViewData["CurrentFilter"] = query;
            ViewData["CurrentCategory"] = categoryId;

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(query))
            {
                queryParams["query"] = query;
            }
            if (categoryId.HasValue)
            {
                queryParams["categoryId"] = categoryId.ToString();
            }
            var requestUri = $"api/VideoArticle/getallvideoarticles?{queryParams}";

            var response = await _httpClient.GetAsync(requestUri);
            var videoArticles = new List<VideoArticleDto>();
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                videoArticles = JsonSerializer.Deserialize<List<VideoArticleDto>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return View(videoArticles);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"api/VideoArticle/{id}");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var videoArticle = JsonSerializer.Deserialize<VideoArticleDto>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            ViewBag.ApiBaseUrl = _configuration.GetValue<string>("ApiSettings:BaseUrl");

            return View(videoArticle);
        }
    }
}
