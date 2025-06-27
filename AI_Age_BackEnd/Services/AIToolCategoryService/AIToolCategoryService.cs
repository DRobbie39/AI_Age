using AI_Age_BackEnd.DTOs.AIToolCategoryDTO;
using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;

namespace AI_Age_BackEnd.Services.AIToolCategoryService
{
    public class AIToolCategoryService
    {
        private readonly IAIToolCategoryRepository _categoryRepository;

        public AIToolCategoryService(
            IAIToolCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<AIToolCategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return categories.Select(c => new AIToolCategoryDto
            {
                CategoryID = c.CategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description,
                CreatedDate = c.CreatedDate.GetValueOrDefault(),
                Status = c.Status.GetValueOrDefault()
            }).ToList();
        }

        public async Task<AIToolCategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException("Không tìm thấy danh mục.");
            }

            return new AIToolCategoryDto
            {
                CategoryID = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description,
                CreatedDate = category.CreatedDate.GetValueOrDefault(),
                Status = category.Status.GetValueOrDefault()
            };
        }

        public async Task<AIToolCategoryDto> CreateCategoryAsync(AIToolCategoryCreateDto dto)
        {
            var existingCategory = await _categoryRepository.GetByNameAsync(dto.CategoryName);
            if (existingCategory != null)
            {
                throw new InvalidOperationException("Tên danh mục này đã tồn tại.");
            }

            var category = new AitoolCategory
            {
                CategoryName = dto.CategoryName,
                Description = dto.Description,
                Status = dto.Status,
                CreatedDate = DateTime.Now
            };

            await _categoryRepository.AddAsync(category);

            return new AIToolCategoryDto
            {
                CategoryID = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description,
                CreatedDate = category.CreatedDate.GetValueOrDefault(),
                Status = category.Status.GetValueOrDefault()
            };
        }

        public async Task<AIToolCategoryDto> UpdateCategoryAsync(AIToolCategoryUpdateDto dto)
        {
            var categoryToUpdate = await _categoryRepository.GetByIdAsync(dto.CategoryID);
            if (categoryToUpdate == null)
            {
                throw new KeyNotFoundException("Không tìm thấy danh mục để cập nhật.");
            }

            var existingCategory = await _categoryRepository.GetByNameAsync(dto.CategoryName);
            if (existingCategory != null && existingCategory.CategoryId != dto.CategoryID)
            {
                throw new InvalidOperationException("Tên danh mục này đã được sử dụng bởi một danh mục khác.");
            }

            categoryToUpdate.CategoryName = dto.CategoryName;
            categoryToUpdate.Description = dto.Description;
            categoryToUpdate.Status = dto.Status;

            await _categoryRepository.UpdateAsync(categoryToUpdate);

            return new AIToolCategoryDto
            {
                CategoryID = categoryToUpdate.CategoryId,
                CategoryName = categoryToUpdate.CategoryName,
                Description = categoryToUpdate.Description,
                CreatedDate = categoryToUpdate.CreatedDate.GetValueOrDefault(),
                Status = categoryToUpdate.Status.GetValueOrDefault()
            };
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var categoryToDelete = await _categoryRepository.GetByIdAsync(id);
            if (categoryToDelete == null)
            {
                throw new KeyNotFoundException("Không tìm thấy danh mục.");
            }

            await _categoryRepository.DeleteAsync(categoryToDelete);
        }
    }
}
