using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AI_Age_BackEnd.Repositories
{
    public class ChatHistoryRepository : IChatHistoryRepository
    {
        private readonly AI_AgeContext _context;

        public ChatHistoryRepository(AI_AgeContext context)
        {
            _context = context;
        }

        public async Task AddChatHistoryAsync(ChatHistory chatHistory)
        {
            await _context.ChatHistories.AddAsync(chatHistory);

            await _context.SaveChangesAsync();
        }

        public async Task<List<ChatHistory>> GetChatHistoryByUserIdAsync(int userId)
        {
            return await _context.ChatHistories
                                 .Where(c => c.UserId == userId)
                                 .OrderBy(c => c.ChatDate)
                                 .ToListAsync();
        }
    }
}
