using Chat.Api.Entities;

namespace Chat.Api.Repositories
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetAllMessages();

        Task<List<Message>> GetChatMessages(Guid chatId);


        Task<Message> GetMessageById(int messageId);

        Task<Message> GetChatMessageById(Guid chatId, int messageId);


        Task AddMessage(Message message);

    }
}
