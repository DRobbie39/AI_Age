using System.ComponentModel.DataAnnotations;

namespace AI_Age_BackEnd.DTOs.AIToolCategoryDTO
{
    public class AIToolCategoryUpdateDto
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }
    }
}
