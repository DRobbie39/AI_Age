using AI_Age_FrontEnd.Areas.Admin.Models.AIToolCategoryViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace AI_Age_FrontEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AIToolCategoryManagementController : Controller
    {
        private readonly HttpClient _httpClient;

        public AIToolCategoryManagementController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/AIToolCategory");
            var categories = new List<AIToolCategoryViewModel>();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                categories = JsonSerializer.Deserialize<List<AIToolCategoryViewModel>>(jsonString, options);
            }
            else
            {
                ViewBag.ErrorMessage = "Không thể tải dữ liệu từ API.";
            }
            return View(categories);
        }

        public IActionResult Create()
        {
            return View(new AIToolCategoryCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AIToolCategoryCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var jsonContent = JsonSerializer.Serialize(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/AIToolCategory", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Thêm mới thể loại thành công!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                var errorDoc = JsonDocument.Parse(errorContent);
                var message = errorDoc.RootElement.GetProperty("message").GetString();
                ModelState.AddModelError(string.Empty, message ?? "Đã có lỗi xảy ra khi thêm mới.");
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/AIToolCategory/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                var category = JsonSerializer.Deserialize<AIToolCategoryUpdateViewModel>(jsonString, options);
                return View(category);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thể loại này.";
            }
            else
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra, không thể lấy dữ liệu để chỉnh sửa.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AIToolCategoryUpdateViewModel model)
        {
            if (id != model.CategoryID)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var jsonContent = JsonSerializer.Serialize(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/AIToolCategory/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Cập nhật thể loại thành công!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                var errorDoc = JsonDocument.Parse(errorContent);
                var message = errorDoc.RootElement.GetProperty("message").GetString();
                ModelState.AddModelError(string.Empty, message ?? "Đã có lỗi xảy ra khi cập nhật.");
                return View(model);
            }
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/AIToolCategory/{id}");

            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Xóa thể loại thành công!" });
            }

            return Json(new { success = false, message = "Có lỗi xảy ra, không thể xóa thể loại." });
        }
    }
}
