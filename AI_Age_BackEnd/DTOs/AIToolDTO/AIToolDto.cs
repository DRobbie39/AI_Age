namespace AI_Age_BackEnd.DTOs.AIToolDTO
{
    public class AIToolDto
    {
        public int ToolID { get; set; }
        public string ToolName { get; set; }
        public string Description { get; set; }
        public string? LogoURL { get; set; }
        public string? WebsiteURL { get; set; }
        public int? CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
