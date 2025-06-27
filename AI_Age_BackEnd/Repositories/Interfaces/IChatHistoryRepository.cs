using AI_Age_BackEnd.Models;

namespace AI_Age_BackEnd.Repositories.Interfaces
{
    public interface IChatHistoryRepository
    {
        Task AddChatHistoryAsync(ChatHistory chatHistory);
        Task<List<ChatHistory>> GetChatHistoryByUserIdAsync(int userId);
    }
}
