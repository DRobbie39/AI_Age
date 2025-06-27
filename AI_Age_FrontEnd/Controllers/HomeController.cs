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

        public IActionResult Details()
        {
            return View();
        }
    }
}
