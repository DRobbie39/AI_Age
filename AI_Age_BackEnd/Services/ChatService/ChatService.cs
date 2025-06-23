using AI_Age_BackEnd.DTOs.ChatDTO;
using OpenAI.Chat;
using System.Text;
using System.Text.Json;

namespace AI_Age_BackEnd.Services.ChatService
{
    public class ChatService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ChatService(IConfiguration configuration)
        {
            _apiKey = configuration["GoogleGemini:ApiKey"];
            Console.WriteLine($"API Key: {_apiKey}");
            _httpClient = new HttpClient();
        }

        public async Task<string> GetChatResponseAsync(ChatDto chatDto)
        {
            // Endpoint của Google Gemini Pro
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent?key={_apiKey}";

            // Cấu trúc request body theo chuẩn của Gemini
            var requestBody = new
            {
                // "contents" là một mảng, chứa lịch sử cuộc trò chuyện
                contents = new object[]
                {
                    // Vai trò của người dùng
                    new {
                        role = "user",
                        parts = new object[] {
                            // System prompt được lồng vào câu hỏi đầu tiên để định hướng cho AI
                            new { text = "Bối cảnh: Bạn là một trợ lý AI thân thiện, chuyên giúp đỡ người lớn tuổi ở Việt Nam. Hãy trả lời mọi câu hỏi bằng tiếng Việt, dùng từ ngữ đơn giản, dễ hiểu, kiên nhẫn và gần gũi như đang nói chuyện với ông bà, cha mẹ. Mục tiêu của bạn là giúp họ làm quen với công nghệ và cảm thấy tự tin hơn khi sử dụng. Dưới đây là câu hỏi của họ:" },
                            new { text = chatDto.Question }
                        }
                    }
                },
                // Cấu hình an toàn và các tham số khác
                generationConfig = new
                {
                    temperature = 0.7,
                    maxOutputTokens = 400 // Tăng lên một chút để có câu trả lời dài hơn nếu cần
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                // Ghi log lỗi để debug dễ hơn
                Console.WriteLine($"Gemini API error: {error}");
                throw new Exception("Lỗi khi kết nối đến trợ lý AI.");
            }

            var responseString = await response.Content.ReadAsStringAsync();

            // Phân tích JSON response của Gemini
            using var jsonDoc = JsonDocument.Parse(responseString);
            // Cấu trúc response của Gemini: candidates[0] -> content -> parts[0] -> text
            var content = jsonDoc.RootElement
                                 .GetProperty("candidates")[0]
                                 .GetProperty("content")
                                 .GetProperty("parts")[0]
                                 .GetProperty("text")
                                 .GetString();

            return content ?? "Xin lỗi, tôi chưa thể trả lời câu hỏi này.";
        }
    }
}
