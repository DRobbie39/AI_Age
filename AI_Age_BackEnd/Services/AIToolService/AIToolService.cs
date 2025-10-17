using AI_Age_BackEnd.DTOs.AIToolDTO;
using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using AI_Age_BackEnd.Repositories;

namespace AI_Age_BackEnd.Services.AIToolService
{
    public class AIToolService
    {
        private readonly IAIToolRepository _toolRepository;
        private readonly IAIToolCategoryRepository _categoryRepository;
        private readonly Cloudinary _cloudinary;

        public AIToolService(
            IAIToolRepository toolRepository,
            IAIToolCategoryRepository categoryRepository,
            Cloudinary cloudinary)
        {
            _toolRepository = toolRepository;
            _categoryRepository = categoryRepository;
            _cloudinary = cloudinary;
        }

        private async Task<string?> UploadLogoToCloudinary(IFormFile? logo)
        {
            if (logo == null || logo.Length == 0)
                return null;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(logo.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                throw new Exception("Định dạng logo không hợp lệ. Chỉ hỗ trợ JPG, PNG.");

            using var stream = logo.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(logo.FileName, stream),
                Folder = "ai_tool_logos", // Thư mục riêng cho logo
                Transformation = new Transformation().Width(200).Height(200).Crop("fit")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception($"Upload logo thất bại: {uploadResult.Error?.Message ?? "Lỗi không xác định"}");

            return uploadResult.SecureUrl.ToString();
        }

        public async Task<AIToolDto> CreateToolAsync(AIToolCreateDto dto)
        {
            if (dto.CategoryID.HasValue)
            {
                var category = await _categoryRepository.GetByIdAsync(dto.CategoryID.Value);
                if (category == null)
                    throw new Exception("Danh mục công cụ AI không tồn tại.");
            }

            var logoUrl = await UploadLogoToCloudinary(dto.Logo);
            var tool = new Aitool
            {
                ToolName = dto.ToolName,
                Description = dto.Description,
                LogoUrl = logoUrl,
                WebsiteUrl = dto.WebsiteURL,
                CategoryId = dto.CategoryID,
                CreatedDate = DateTime.Now
            };

            await _toolRepository.AddAsync(tool);
            var createdTool = await _toolRepository.GetByIdAsync(tool.ToolId);

            return new AIToolDto
            {
                ToolID = createdTool.ToolId,
                ToolName = createdTool.ToolName,
                Description = createdTool.Description,
                LogoURL = createdTool.LogoUrl,
                WebsiteURL = createdTool.WebsiteUrl,
                CategoryID = createdTool.CategoryId,
                CategoryName = createdTool.Category?.CategoryName,
                CreatedDate = createdTool.CreatedDate.GetValueOrDefault()
            };
        }

        public async Task<AIToolDto> GetToolByIdAsync(int id)
        {
            var tool = await _toolRepository.GetByIdAsync(id);
            if (tool == null)
                throw new KeyNotFoundException("Không tìm thấy công cụ AI.");

            return new AIToolDto
            {
                ToolID = tool.ToolId,
                ToolName = tool.ToolName,
                Description = tool.Description,
                LogoURL = tool.LogoUrl,
                WebsiteURL = tool.WebsiteUrl,
                CategoryID = tool.CategoryId,
                CategoryName = tool.Category?.CategoryName,
                CreatedDate = tool.CreatedDate.GetValueOrDefault()
            };
        }

        public async Task<List<AIToolDto>> GetAllToolsAsync(string? searchQuery = null, int? categoryId = null)
        {
            var tools = await _toolRepository.GetAllAsync(searchQuery, categoryId);

            return tools.Select(tool => new AIToolDto
            {
                ToolID = tool.ToolId,
                ToolName = tool.ToolName,
                Description = tool.Description,
                LogoURL = tool.LogoUrl,
                WebsiteURL = tool.WebsiteUrl,
                CategoryID = tool.CategoryId,
                CategoryName = tool.Category?.CategoryName,
                CreatedDate = tool.CreatedDate.GetValueOrDefault()
            }).ToList();
        }

        public async Task<AIToolDto> UpdateToolAsync(AIToolUpdateDto dto)
        {
            var existingTool = await _toolRepository.GetByIdAsync(dto.ToolID);
            if (existingTool == null)
                throw new KeyNotFoundException("Không tìm thấy công cụ AI để cập nhật.");

            if (dto.CategoryID.HasValue)
            {
                var category = await _categoryRepository.GetByIdAsync(dto.CategoryID.Value);
                if (category == null)
                    throw new Exception("Danh mục công cụ AI không tồn tại.");
            }

            var newLogoUrl = dto.Logo != null ? await UploadLogoToCloudinary(dto.Logo) : existingTool.LogoUrl;
            existingTool.ToolName = dto.ToolName;
            existingTool.Description = dto.Description;
            existingTool.LogoUrl = newLogoUrl;
            existingTool.WebsiteUrl = dto.WebsiteURL;
            existingTool.CategoryId = dto.CategoryID;

            await _toolRepository.UpdateAsync(existingTool);

            var updatedTool = await _toolRepository.GetByIdAsync(existingTool.ToolId);

            return new AIToolDto
            {
                ToolID = updatedTool.ToolId,
                ToolName = updatedTool.ToolName,
                Description = updatedTool.Description,
                LogoURL = updatedTool.LogoUrl,
                WebsiteURL = updatedTool.WebsiteUrl,
                CategoryID = updatedTool.CategoryId,
                CategoryName = updatedTool.Category?.CategoryName,
                CreatedDate = updatedTool.CreatedDate.GetValueOrDefault()
            };
        }

        public async Task DeleteToolAsync(int id)
        {
            var tool = await _toolRepository.GetByIdAsync(id);
            if (tool == null)
                throw new KeyNotFoundException("Không tìm thấy công cụ AI.");

            await _toolRepository.DeleteAsync(tool);
        }

        public async Task<List<AIToolDto>> GetToolsByCategoryIdAsync(int categoryId)
        {
            var tools = await _toolRepository.GetByCategoryIdAsync(categoryId);

            return tools.Select(t => new AIToolDto
            {
                ToolID = t.ToolId,
                ToolName = t.ToolName,
                Description = t.Description,
                LogoURL = t.LogoUrl
            }).ToList();
        }
    }
}
