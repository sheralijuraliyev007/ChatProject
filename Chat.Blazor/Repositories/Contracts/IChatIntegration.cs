using System.Net;

namespace Chat.Blazor.Repositories.Contracts
{
    public interface IChatIntegration
    {
        Task<Tuple<HttpStatusCode, object>> GetUserChats();

        Task<Tuple<HttpStatusCode, object>> GetUserChat(Guid toUserId);


        Task<Tuple<HttpStatusCode, object>> GetChatMessages(Guid chatId);

        Task<Tuple<HttpStatusCode, object>> SendTextMessage(Guid chatId, string text);



    }
}
