using System.ComponentModel.DataAnnotations;

namespace Chat.Client.Models.UserModels
{
    public class LoginUserModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
