
using Chat.Blazor.DTOs;

namespace Chat.Blazor.DTOs
{
    public class ChatDto
    {

        public Guid Id { get; set; }

        public List<string>? ChatNames { get; set; }

        public List<UserChatDto>? UserChats { get; set; }

        public List<MessageDto>? Messages { get; set; }
    }
}
