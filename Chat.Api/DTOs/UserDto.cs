using System.ComponentModel.DataAnnotations;
using Chat.Api.Entities;

namespace Chat.Api.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Firstname { get; set; }


        public string? Lastname { get; set; }

        public string Username { get; set; }

        public string Gender { get; set; }


        public string? Bio { get; set; }

        public string? Role { get; set; }


        public byte[]? PhotoData { get; set; }


        public DateTime CreatedAt { get; set; }


        public byte? Age { get; set; }

        public List<UserChatDto>? UserChats { get; set; }
    }
}
