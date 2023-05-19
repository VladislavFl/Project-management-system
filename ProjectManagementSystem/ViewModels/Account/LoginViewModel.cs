using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Не указан e-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
