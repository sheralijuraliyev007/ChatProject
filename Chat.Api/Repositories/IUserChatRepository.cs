using Chat.Api.Entities;

namespace Chat.Api.Repositories
{
    public interface IUserChatRepository
    {
        Task AddUserChat(UserChat userChat);

        Task DeleteUserChat(UserChat userChat);

        Task InUserChat(Guid chatId, Guid userId);

        Task<UserChat> GetUserChat(Guid chatId, Guid userId);
    }
}
