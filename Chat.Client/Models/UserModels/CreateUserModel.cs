using System.ComponentModel.DataAnnotations;

namespace Chat.Client.Models.UserModels
{
    public class CreateUserModel
    {
        [Required]
        public string Firstname { get; set; }


        public string? Lastname { get; set; }

        [Required]
        public string Username { get; set; }


        [Required]
        public string Password{ get; set; }


        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }


        [Required]
        public string Gender { get; set; }
    }
}
