using Chat.Api.Entities;
using System.ComponentModel.DataAnnotations;

namespace Chat.Api.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }

        public string Text { get; set; }

         public string FromUserName { get; set; }

        public int ContentId { get; set; }

        public ContentDto? Content { get; set; }

        public Guid ChatId { get; set; }


        public bool IsEdited { get; set; }

        public DateTime SendTime => DateTime.UtcNow;

        public DateTime? EditedAt { get; set; }
    }
}
