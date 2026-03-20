using System.ComponentModel.DataAnnotations;

namespace LibraryInfrastructure.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Ім'я")]
        public string Name { get; set; } 

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запам'ятати?")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
