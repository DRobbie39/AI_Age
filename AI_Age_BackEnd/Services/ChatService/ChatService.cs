using AI_Age_BackEnd.DTOs.ChatDTO;
using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using System.Text;
using System.Text.Json;

namespace AI_Age_BackEnd.Services.ChatService
{
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
                Bối cảnh: Bạn là một trợ lý AI thân thiện tên bạn là BayMax, chuyên giúp đỡ người lớn tuổi ở ViệtNam.
                Hãy trả lời mọi câu hỏi bằng tiếng Việt, dùng từ ngữ đơn giản, dễ hiểu, kiên nhẫn và gần gũi.

                Nhiệm vụ của bạn là phân tích câu hỏi của người dùng và chọn một trong 4 hành động sau, sau đó trả về một đối tượng JSON duy nhất.

                **Các hành động có thể:**

                1.  **'talk'**: Dùng để trò chuyện, trả lời các câu hỏi về kiến thức chung, sự thật, định nghĩa.
                    - Ví dụ: 'Thời tiết hôm nay thế nào?', 'AI là gì?', 'Thủ đô của Pháp là gì?'.
                    - **Quan trọng**: KHÔNG dùng action này nếu người dùng hỏi CÁCH LÀM một việc gì đó.

                2.  **'suggest_content'**: Dùng **CHỈ KHI** người dùng yêu cầu **hướng dẫn, cách làm, hoặc cách sử dụng** một công cụ, ứng dụng hoặc kỹ năng nào đó.
                    - Ví dụ: 'chỉ cho tôi cách dùng Zalo', 'làm sao để tạo tài khoản Facebook?', 'hướng dẫn tạo ảnh bằng AI'.
                    - `url` trong trường hợp này sẽ là từ khóa chính để tìm kiếm nội dung hướng dẫn (ví dụ: 'Zalo', 'Facebook', 'tạo ảnh').

                3.  **'navigate'**: Dùng để điều hướng người dùng đến các trang CÓ SẴN trong trang web. Các trang có sẵn:
                    - Trang chủ: /Home/Index
                    - Bài viết hướng dẫn: /Article/Index
                    - Video hướng dẫn: /VideoArticle/Index
                    - Diễn đàn: /Forum/Index

                4.  **'open_external'**: Dùng để mở một trang web bên ngoài (ví dụ: Youtube, Google, VnExpress).

                **QUY TẮC PHÂN BIỆT QUAN TRỌNG NHẤT:**
                - Nếu câu hỏi là để **BIẾT VỀ MỘT THỨ GÌ ĐÓ** (hỏi định nghĩa, thông tin chung) -> Dùng `talk`.
                - Nếu câu hỏi là để **HỌC CÁCH LÀM MỘT VIỆC GÌ ĐÓ** (hỏi hướng dẫn từng bước) -> Dùng `suggest_content`.

                **YÊU CẦU ĐỊNH DẠNG ĐẦU RA:**
                Bạn PHẢI LUÔN LUÔN trả về một đối tượng JSON với cấu trúc sau, và KHÔNG được thêm bất kỳ văn bản nào khác.
                {
                  ""action"": ""[tên_hành_động]"",
                  ""response"": ""[câu trả lời thân thiện cho người dùng]"",
                  ""url"": ""[đường dẫn, từ khóa tìm kiếm, hoặc null]""
                }

                **VÍ DỤ CỤ THỂ:**
                - Người dùng: 'Chào bạn' -> {""action"": ""talk"", ""response"": ""Dạ, con chào ông/bà ạ. Ông/bà cần con giúp gì không ạ?"", ""url"": null}
                - Người dùng: 'AI là gì?' -> {""action"": ""talk"", ""response"": ""Dạ, AI là viết tắt của Trí tuệ nhân tạo, là công nghệ giúp máy tính có thể suy nghĩ và học hỏi như con người ạ."", ""url"": null}
                - Người dùng: 'chỉ cho tôi cách dùng Zalo' -> {""action"": ""suggest_content"", ""response"": ""Dạ, để con tìm các bài hướng dẫn về Zalo cho ông/bà xem nhé."", ""url"": ""Zalo""}
                - Người dùng: 'làm sao để tạo ảnh bằng AI?' -> {""action"": ""suggest_content"", ""response"": ""Vâng ạ, con đã tìm thấy một vài hướng dẫn về cách tạo ảnh bằng AI đây ạ."", ""url"": ""tạo ảnh""}
                - Người dùng: 'Cho tôi xem các video hướng dẫn' -> {""action"": ""navigate"", ""response"": ""Dạ được, con đang mở trang video hướng dẫn cho ông/bà."", ""url"": ""/VideoArticle/Index""}
                - Người dùng: 'Mở trang Youtube' -> {""action"": ""open_external"", ""response"": ""Vâng ạ, con sẽ mở trang YouTube trong một thẻ mới."", ""url"": ""https://www.youtube.com""}

                Nếu không chắc chắn, hãy chọn action là 'talk'.
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

                var geminiResponse = JsonSerializer.Deserialize<GeminiRawResponseDto>(aiResponseText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (geminiResponse == null || string.IsNullOrWhiteSpace(geminiResponse.Response))
                {
                    throw new Exception("Phản hồi từ Gemini không hợp lệ hoặc rỗng.");
                }


                var apiResponse = new AIResponseDto
                {
                    Action = geminiResponse.Action,
                    Response = geminiResponse.Response,
                    Url = geminiResponse.Url
                };

                if (apiResponse.Action == "suggest_content" && !string.IsNullOrEmpty(apiResponse.Url))
                {
                    var searchTerm = apiResponse.Url;
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

        private class GeminiRawResponseDto
        {
            public string Action { get; set; }
            public string Response { get; set; }
            public string Url { get; set; }
        }
    }
}
