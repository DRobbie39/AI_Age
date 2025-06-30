using System.Text.Json;
using AI_Age_FrontEnd.Models.AIToolViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AI_Age_FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HomeController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var viewModel = new HomeViewModel();

            // Truyền API Base URL cho View để sử dụng trong JavaScript
            ViewBag.ApiBaseUrl = _configuration.GetValue<string>("ApiSettings:BaseUrl");

            var categoriesResponse = await _httpClient.GetAsync("api/AIToolCategory");
            if (categoriesResponse.IsSuccessStatusCode)
            {
                var categoriesJson = await categoriesResponse.Content.ReadAsStringAsync();
                viewModel.Categories = JsonSerializer.Deserialize<List<AIToolCategoryViewModel>>(categoriesJson, jsonOptions) ?? new List<AIToolCategoryViewModel>();
            }

            var toolsResponse = await _httpClient.GetAsync("api/AITool");
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

            var toolResponse = await _httpClient.GetAsync($"api/AITool/{id}");
            if (toolResponse.IsSuccessStatusCode)
            {
                var toolJson = await toolResponse.Content.ReadAsStringAsync();
                viewModel.Tool = JsonSerializer.Deserialize<AIToolViewModel>(toolJson, jsonOptions);
            }
            else
            {
                return NotFound();
            }

            var articlesResponse = await _httpClient.GetAsync($"api/Article/ByTool/{id}");
            if (articlesResponse.IsSuccessStatusCode)
            {
                var articlesJson = await articlesResponse.Content.ReadAsStringAsync();
                viewModel.Articles = JsonSerializer.Deserialize<List<ArticleViewModel>>(articlesJson, jsonOptions) ?? new List<ArticleViewModel>();
            }

            var videosResponse = await _httpClient.GetAsync($"api/VideoArticle/ByTool/{id}");
            if (videosResponse.IsSuccessStatusCode)
            {
                var videosJson = await videosResponse.Content.ReadAsStringAsync();
                viewModel.Videos = JsonSerializer.Deserialize<List<VideoArticleViewModel>>(videosJson, jsonOptions) ?? new List<VideoArticleViewModel>();
            }

            return View(viewModel);
        }
    }
}
