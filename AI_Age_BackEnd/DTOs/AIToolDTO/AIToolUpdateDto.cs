namespace AI_Age_BackEnd.DTOs.AIToolDTO
{
    public class AIToolUpdateDto
    {
        public int ToolID { get; set; }
        public string ToolName { get; set; }
        public string Description { get; set; }

        public string? WebsiteURL { get; set; }
        public int? CategoryID { get; set; }
        public bool Status { get; set; }
        public IFormFile? Logo { get; set; }
    }
}
