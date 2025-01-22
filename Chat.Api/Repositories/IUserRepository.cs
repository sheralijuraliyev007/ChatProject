using Chat.Api.Entities;

namespace Chat.Api.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();

        Task<User> GetUserByIdAsync(Guid id);

        Task<User?> GetUserByUsernameAsync(string name);


        Task AddUser(User user);

        Task UpdateUser(User user);

        Task DeleteUser(User user);


    }
}
