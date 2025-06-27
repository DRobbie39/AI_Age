using System.Text.Json;
using AI_Age_FrontEnd.Models.AIToolViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AI_Age_FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7022/api/");
        }

        public async Task<IActionResult> Index()
        {
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var viewModel = new HomeViewModel();

            var categoriesResponse = await _httpClient.GetAsync("AIToolCategory");
            if (categoriesResponse.IsSuccessStatusCode)
            {
                var categoriesJson = await categoriesResponse.Content.ReadAsStringAsync();
                viewModel.Categories = JsonSerializer.Deserialize<List<AIToolCategoryViewModel>>(categoriesJson, jsonOptions) ?? new List<AIToolCategoryViewModel>();
            }

            var toolsResponse = await _httpClient.GetAsync("AITool");
            if (toolsResponse.IsSuccessStatusCode)
            {
                var toolsJson = await toolsResponse.Content.ReadAsStringAsync();
                viewModel.AITools = JsonSerializer.Deserialize<List<AIToolViewModel>>(toolsJson, jsonOptions) ?? new List<AIToolViewModel>();
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var viewModel = new AIToolDetailViewModel();

            // 1. Lấy thông tin chi tiết của công cụ
            var toolResponse = await _httpClient.GetAsync($"AITool/{id}");
            if (toolResponse.IsSuccessStatusCode)
            {
                var toolJson = await toolResponse.Content.ReadAsStringAsync();
                viewModel.Tool = JsonSerializer.Deserialize<AIToolViewModel>(toolJson, jsonOptions);
            }
            else
            {
                // Nếu không tìm thấy công cụ, có thể chuyển hướng về trang lỗi hoặc trang chủ
                return NotFound();
            }

            // 2. Lấy danh sách bài viết liên quan
            var articlesResponse = await _httpClient.GetAsync($"Article/ByTool/{id}");
            if (articlesResponse.IsSuccessStatusCode)
            {
                var articlesJson = await articlesResponse.Content.ReadAsStringAsync();
                viewModel.Articles = JsonSerializer.Deserialize<List<ArticleViewModel>>(articlesJson, jsonOptions) ?? new List<ArticleViewModel>();
            }

            // 3. Lấy danh sách video liên quan
            var videosResponse = await _httpClient.GetAsync($"VideoArticle/ByTool/{id}");
            if (videosResponse.IsSuccessStatusCode)
            {
                var videosJson = await videosResponse.Content.ReadAsStringAsync();
                viewModel.Videos = JsonSerializer.Deserialize<List<VideoArticleViewModel>>(videosJson, jsonOptions) ?? new List<VideoArticleViewModel>();
            }

            return View(viewModel);
        }
    }
}
