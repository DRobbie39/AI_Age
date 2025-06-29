namespace AI_Age_BackEnd.DTOs.ChatDTO
{
    public class AIResponseDto
    {
        public string Action { get; set; }
        public string Response { get; set; }
        public string? Url { get; set; }
        public List<SearchResultDto> Suggestions { get; set; } = new List<SearchResultDto>();
    }
}
