using System.ComponentModel.DataAnnotations;

namespace FeedApp.UI.ViewModels
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Публичное имя не должно быть пустым")]
        [MinLength(3, ErrorMessage = "Минимум 3 символа")]
        public string PublicName { get; set; }
        
        [Required(ErrorMessage = "Логин не должен быть пустым")]
        [MinLength(6, ErrorMessage = "Минимум 6 символов")]
        public string Login { get; set; }
        
        [Required(ErrorMessage = "Пароль не должен быть пустым")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Минимум 6 символов")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Подтверждение пароля не должно быть пустым")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [MinLength(6, ErrorMessage = "Минимум 6 символов")]
        public string PasswordConfirmation { get; set; }
    }
}