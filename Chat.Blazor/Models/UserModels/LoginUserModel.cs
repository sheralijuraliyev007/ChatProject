using System.ComponentModel.DataAnnotations;

namespace Chat.Blazor.Models.UserModels
{
    public class LoginUserModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
