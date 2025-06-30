using AI_Age_FrontEnd.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Web;

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

        public async Task<IActionResult> Index(string? query, int? categoryId)
        {
            // 1. Lấy danh sách thể loại cho bộ lọc
            try
            {
                var categoryResponse = await _httpClient.GetAsync("api/ArticleCategory/getallcategories");
                categoryResponse.EnsureSuccessStatusCode();
                var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
                ViewBag.Categories = JsonSerializer.Deserialize<List<ArticleCategoryDto>>(categoryJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                ViewBag.Categories = new List<ArticleCategoryDto>(); // Nếu lỗi thì trả về list rỗng
            }

            // 2. Lưu lại các filter hiện tại để hiển thị trên View
            ViewData["CurrentFilter"] = query;
            ViewData["CurrentCategory"] = categoryId;

            // 3. Xây dựng URI để gọi API lấy bài viết đã lọc
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(query))
            {
                queryParams["query"] = query;
            }
            if (categoryId.HasValue)
            {
                queryParams["categoryId"] = categoryId.ToString();
            }
            var requestUri = $"api/Article/getallarticles?{queryParams}";


            // 4. Gọi API và lấy danh sách bài viết
            var response = await _httpClient.GetAsync(requestUri);
            var articles = new List<ArticleDto>();
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                articles = JsonSerializer.Deserialize<List<ArticleDto>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

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
