using System.ComponentModel.DataAnnotations;

namespace Chat.Api.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        public string Firstname { get; set; }


        public string? Lastname { get; set; }

        [Required]
        public string Username { get; set; }


        [Required]
        public string PasswordHash { get; set; }


        [Required]
        public string Gender { get; set; }


        public string? Bio { get; set; }


        public byte[]? PhotoData { get; set; }


        public DateTime  CreatedAt =>DateTime.UtcNow;


        public byte? Age { get; set; }


        public string? Role { get; set; }

        public List<UserChat>? UserChats { get; set; }
    }
}
