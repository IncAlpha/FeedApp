using System.ComponentModel.DataAnnotations;

namespace FeedIt.UI.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Требуется указать никнейм.")]
        [StringLength(50)]
        public string PublicName { get; set; }
        
        [Required(ErrorMessage = "Требуется указать логин.")]
        [StringLength(20)]
        public string Login { get; set; }
        
        [Required(ErrorMessage = "Требуется указать пароль.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Подтвердите пароль.")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        [DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; }
    }
}