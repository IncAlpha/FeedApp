using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Serialization;

namespace FeedIt.UI.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Требуется указать никнейм.")]
        [MaxLength(50, ErrorMessage = "Не больше 50 символов.")]
        public string PublicName { get; set; }
        
        [Required(ErrorMessage = "Требуется указать логин.")]
        [MaxLength(50, ErrorMessage = "Не больше 50 символов.")]
        [MinLength(6, ErrorMessage = "не меньше 6 символов")]
        public string Login { get; set; }
        
        [Required(ErrorMessage = "Требуется указать пароль.")]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage = "Минимум 6 символов")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Подтвердите пароль.")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        [DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; }
    }
}