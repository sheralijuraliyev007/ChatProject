using System.ComponentModel.DataAnnotations;

namespace Chat.Blazor.DTOs
{
    public class ContentDto
    {
        public int Id { get; set; }

        public string? Caption { get; set; }

        [Required]
        public string FileUrl { get; set; }

        public string? Type { get; set; }

        public int MessageId { get; set; }
    }
}
