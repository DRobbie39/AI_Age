using AI_Age_FrontEnd.Areas.Admin.Models.UserViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AI_Age_FrontEnd.Areas.Admin.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserManagementController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Area("Admin")]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("BackendApi");
            var response = await client.GetAsync("users");

            List<UserViewModel> users = new();

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                users = await JsonSerializer.DeserializeAsync<List<UserViewModel>>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                ViewBag.ErrorMessage = "Không thể tải dữ liệu người dùng từ API.";
            }

            return View(users);
        }
    }
}
