using System.ComponentModel.DataAnnotations;

namespace Chat.Blazor.Models
{
    public class TextModel
    {
        [Required]
        public string Text { get; set; }
    }
}
