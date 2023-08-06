using System.ComponentModel.DataAnnotations;

namespace _90Zone.App.Dto {
    public class UserRegistrationRequestDto {

        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        //[EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }

    public class UserLoginRequestDto {

        //[EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }

}
