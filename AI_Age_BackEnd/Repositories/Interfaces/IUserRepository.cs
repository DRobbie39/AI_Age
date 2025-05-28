using AI_Age_BackEnd.Models;

namespace AI_Age_BackEnd.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task AddUserAsync(User user);
    }
}
