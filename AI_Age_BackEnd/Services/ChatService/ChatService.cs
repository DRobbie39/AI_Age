using AI_Age_BackEnd.DTOs.ChatDTO;
using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using System.Text;
using System.Text.Json;

namespace AI_Age_BackEnd.Services.ChatService
{
    public class AIResponseDto
    {
        public string Action { get; set; }
        public string Response { get; set; }
        public string Url { get; set; }
        public List<SearchResultDto> Suggestions { get; set; } = new List<SearchResultDto>();
    }

    public class ChatService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly IChatHistoryRepository _chatHistoryRepository;
        private readonly ISearchRepository _searchRepository;

        public ChatService(IConfiguration configuration, IChatHistoryRepository chatHistoryRepository, ISearchRepository searchRepository)
        {
            _apiKey = configuration["GoogleGemini:ApiKey"];
            _httpClient = new HttpClient();
            _chatHistoryRepository = chatHistoryRepository;
            _searchRepository = searchRepository;
        }

        public async Task<AIResponseDto> GetChatResponseAsync(ChatDto chatDto, int userId)
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent?key={_apiKey}";

            var systemInstruction = @"
                Bối cảnh: Bạn là một trợ lý AI thân thiện, chuyên giúp đỡ người lớn tuổi ở Việt Nam.
                Hãy trả lời mọi câu hỏi bằng tiếng Việt, dùng từ ngữ đơn giản, dễ hiểu, kiên nhẫn và gần gũi.

                Bạn có 4 khả năng chính:
                1. 'talk': Trả lời câu hỏi thông thường.
                2. 'navigate': Điều hướng trong trang web. Các trang có sẵn:
                    - Trang chủ: /Home/Index
                    - Bài viết hướng dẫn: /Article/Index
                    - Video hướng dẫn: /VideoArticle/Index
                    - Diễn đàn: /Forum/Index
                3. 'open_external': Mở một trang web bên ngoài.
                4. 'suggest_content': Khi người dùng muốn tìm hiểu/hỏi về cách sử dụng một công cụ AI hoặc một chủ đề cụ thể trên trang web.

                DỰA VÀO CÂU HỎI CỦA NGƯỜI DÙNG, BẠN PHẢI LUÔN LUÔN TRẢ VỀ MỘT ĐỐI TƯỢNG JSON VỚI CẤU TRÚC SAU:
                {
                  ""action"": ""[tên_hành_động]"",
                  ""response"": ""[câu trả lời thân thiện cho người dùng]"",
                  ""url"": ""[đường dẫn nếu có, hoặc từ khóa tìm kiếm cho 'suggest_content']""
                }

                VÍ DỤ:
                - Người dùng hỏi: 'Thời tiết hôm nay thế nào?' -> {""action"": ""talk"", ""response"": ""Dạ, thời tiết hôm nay ở [địa điểm] là..."", ""url"": null}
                - Người dùng hỏi: 'Cho tôi xem các bài viết hướng dẫn' -> {""action"": ""navigate"", ""response"": ""Dạ được, con đang mở trang bài viết cho ông/bà."", ""url"": ""/Article/Index""}
                - Người dùng hỏi: 'Mở trang Youtube' -> {""action"": ""open_external"", ""response"": ""Vâng ạ, con sẽ mở trang YouTube trong một thẻ mới."", ""url"": ""https://www.youtube.com""}
                - Người dùng hỏi: 'chỉ cho tôi cách dùng Zalo' -> {""action"": ""suggest_content"", ""response"": ""Dạ, để con tìm các bài hướng dẫn về Zalo cho ông/bà xem nhé."", ""url"": ""Zalo""}
                - Người dùng hỏi: 'làm sao để tạo ảnh bằng AI?' -> {""action"": ""suggest_content"", ""response"": ""Vâng ạ, con đã tìm thấy một vài hướng dẫn về cách tạo ảnh bằng AI đây ạ."", ""url"": ""tạo ảnh""}

                Nếu không chắc chắn, hãy chọn action là 'talk'. Chỉ trả về DUY NHẤT đối tượng JSON, không thêm bất kỳ văn bản nào khác.
                Dưới đây là câu hỏi của họ:
            ";

            var requestBody = new
            {
                contents = new object[]
                {
                    new {
                        role = "user",
                        parts = new object[] {
                            new { text = systemInstruction },
                            new { text = chatDto.Question }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.5,
                    maxOutputTokens = 400,
                    response_mime_type = "application/json"
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Gemini API error: {error}");
                throw new Exception("Lỗi khi kết nối đến trợ lý AI.");
            }

            var responseString = await response.Content.ReadAsStringAsync();

            try
            {
                using var jsonDoc = JsonDocument.Parse(responseString);
                var aiResponseText = jsonDoc.RootElement
                                            .GetProperty("candidates")[0]
                                            .GetProperty("content")
                                            .GetProperty("parts")[0]
                                            .GetProperty("text")
                                            .GetString();

                var aiResponse = JsonSerializer.Deserialize<AIResponseDto>(aiResponseText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (aiResponse == null || string.IsNullOrWhiteSpace(aiResponse.Response))
                {
                    throw new Exception("Phản hồi từ Gemini không hợp lệ hoặc rỗng.");
                }


                var apiResponse = new AIResponseDto
                {
                    Action = aiResponse.Action,
                    Response = aiResponse.Response,
                    Url = aiResponse.Url
                };

                if (aiResponse.Action == "suggest_content" && !string.IsNullOrEmpty(aiResponse.Url))
                {
                    var searchTerm = aiResponse.Url;
                    var searchResults = await _searchRepository.SearchContentAsync(searchTerm);
                    apiResponse.Suggestions = searchResults;

                    if (searchResults.Count == 0)
                    {
                        apiResponse.Response = $"Con xin lỗi, hiện tại con chưa tìm thấy bài hướng dẫn nào về '{searchTerm}' cả. Ông/Bà có thể hỏi con về một chủ đề khác được không ạ?";

                        apiResponse.Action = "talk";
                    }
                }

                var chatHistory = new ChatHistory
                {
                    UserId = userId,
                    Question = chatDto.Question,
                    Response = apiResponse.Response,
                    ChatDate = DateTime.UtcNow
                };
 
                await _chatHistoryRepository.AddChatHistoryAsync(chatHistory);

                return apiResponse ?? throw new Exception("Không thể phân tích phản hồi từ AI.");
            }
            catch (Exception ex)
            {
                return new AIResponseDto
                {
                    Action = "talk",
                    Response = "Con xin lỗi, đã có lỗi xảy ra. Ông/Bà vui lòng thử lại sau ạ.",
                    Url = null
                };
            }
        }

        public async Task<List<ChatDto>> GetChatHistoryAsync(int userId)
        {
            var history = await _chatHistoryRepository.GetChatHistoryByUserIdAsync(userId);

            var historyDto = history.Select(h => new ChatDto
            {
                Question = h.Question,
                Response = h.Response,
                ChatDate = h.ChatDate
            }).ToList();

            return historyDto;
        }
    }
}
