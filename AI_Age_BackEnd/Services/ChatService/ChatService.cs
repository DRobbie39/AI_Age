using AI_Age_BackEnd.DTOs.ChatDTO;
using OpenAI.Chat;
using System.Text;

namespace AI_Age_BackEnd.Services.ChatService
{
    public class ChatService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public ChatService(IConfiguration configuration)
        {
            _apiKey = configuration["OpenAI:ApiKey"];
            _baseUrl = configuration["OpenAI:BaseUrl"];
            _httpClient = new HttpClient();
        }

        public async Task<string> GetChatResponseAsync(ChatDto chatDto)
        {
            var requestBody = new
            {
                model = "llama3-70b-8192",
                messages = new object[]
                {
                new { role = "system", content = "trả lời bằng tiếng Việt và tập trung vào việc giúp người lớn tuổi tiếp cận công nghệ AI. Hãy cung cấp câu trả lời đơn giản, dễ hiểu, và phù hợp với nhu cầu của người lớn tuổi, giải thích các khái niệm công nghệ một cách gần gũi." },
                new { role = "user", content = chatDto.Question }
                },
                temperature = 0.7,
                max_tokens = 200
            };

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/v1/chat/completions");
            request.Headers.Add("Authorization", $"Bearer {_apiKey}");
            request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Groq API error: {error}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            using var jsonDoc = System.Text.Json.JsonDocument.Parse(responseString);
            var content = jsonDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return content;
        }
    }
}
