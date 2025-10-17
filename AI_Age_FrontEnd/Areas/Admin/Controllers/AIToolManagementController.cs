using AI_Age_FrontEnd.Areas.Admin.Models.AIToolCategoryViewModel;
using AI_Age_FrontEnd.Areas.Admin.Models.AIToolViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace AI_Age_FrontEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AIToolManagementController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;

        public AIToolManagementController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        private async Task<List<SelectListItem>> GetCategoriesForDropdown()
        {
            var response = await _httpClient.GetAsync("api/AIToolCategory");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var categories = JsonSerializer.Deserialize<List<AIToolCategoryViewModel>>(jsonString, _options);
                return categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryID.ToString(),
                    Text = c.CategoryName
                }).ToList();
            }
            return new List<SelectListItem>();
        }

        public async Task<IActionResult> Index(string searchQuery)
        {
            ViewData["CurrentFilter"] = searchQuery;
            var requestUri = "api/AITool";
            if (!string.IsNullOrEmpty(searchQuery))
            {
                requestUri += $"?searchQuery={Uri.EscapeDataString(searchQuery)}";
            }

            var response = await _httpClient.GetAsync(requestUri);
            var tools = new List<AIToolViewModel>();
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                tools = JsonSerializer.Deserialize<List<AIToolViewModel>>(jsonString, _options);
            }
            else
            {
                ViewBag.ErrorMessage = "Không thể tải dữ liệu công cụ AI.";
            }
            return View(tools);
        }

        public async Task<IActionResult> Create()
        {
            var model = new AIToolCreateViewModel
            {
                Categories = await GetCategoriesForDropdown()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AIToolCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategoriesForDropdown();
                return View(model);
            }

            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(model.ToolName), nameof(model.ToolName));
            content.Add(new StringContent(model.Description), nameof(model.Description));
            if (model.WebsiteURL != null)
                content.Add(new StringContent(model.WebsiteURL), nameof(model.WebsiteURL));
            if (model.CategoryID.HasValue)
                content.Add(new StringContent(model.CategoryID.Value.ToString()), nameof(model.CategoryID));

            if (model.Logo != null)
            {
                var streamContent = new StreamContent(model.Logo.OpenReadStream());
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(model.Logo.ContentType);
                content.Add(streamContent, nameof(model.Logo), model.Logo.FileName);
            }

            var response = await _httpClient.PostAsync("api/AITool", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Thêm mới công cụ thành công!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, "Đã có lỗi xảy ra: " + errorContent);
                model.Categories = await GetCategoriesForDropdown();
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/AITool/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Không tìm thấy công cụ AI.";
                return RedirectToAction(nameof(Index));
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var toolDto = JsonSerializer.Deserialize<AIToolViewModel>(jsonString, _options);

            var model = new AIToolUpdateViewModel
            {
                ToolID = toolDto.ToolID,
                ToolName = toolDto.ToolName,
                Description = toolDto.Description,
                WebsiteURL = toolDto.WebsiteURL,
                CategoryID = toolDto.CategoryID,
                ExistingLogoUrl = toolDto.LogoURL,
                Categories = await GetCategoriesForDropdown()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AIToolUpdateViewModel model)
        {
            if (id != model.ToolID) return BadRequest();

            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategoriesForDropdown();
                return View(model);
            }

            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(model.ToolID.ToString()), nameof(model.ToolID));
            content.Add(new StringContent(model.ToolName), nameof(model.ToolName));
            content.Add(new StringContent(model.Description), nameof(model.Description));
            if (model.WebsiteURL != null)
                content.Add(new StringContent(model.WebsiteURL), nameof(model.WebsiteURL));
            if (model.CategoryID.HasValue)
                content.Add(new StringContent(model.CategoryID.Value.ToString()), nameof(model.CategoryID));

            if (model.Logo != null)
            {
                var streamContent = new StreamContent(model.Logo.OpenReadStream());
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(model.Logo.ContentType);
                content.Add(streamContent, nameof(model.Logo), model.Logo.FileName);
            }

            var response = await _httpClient.PutAsync($"api/AITool/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Cập nhật công cụ thành công!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, "Đã có lỗi xảy ra: " + errorContent);
                model.Categories = await GetCategoriesForDropdown();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/AITool/{id}");

            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Xóa công cụ thành công!" });
            }

            return Json(new { success = false, message = "Có lỗi xảy ra, không thể xóa công cụ." });
        }
    }
}
