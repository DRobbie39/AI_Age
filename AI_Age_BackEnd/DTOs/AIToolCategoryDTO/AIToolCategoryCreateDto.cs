using System.ComponentModel.DataAnnotations;

namespace AI_Age_BackEnd.DTOs.AIToolCategoryDTO
{
    public class AIToolCategoryCreateDto
    {
        public string CategoryName { get; set; }
        public string? Description { get; set; }
    }
}
