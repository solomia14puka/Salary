using System.ComponentModel.DataAnnotations;

namespace LibraryInfrastructure.ViewModels 
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Поле 'Ім'я' обов'язкове для заповнення")]
        [Display(Name = "Ім'я")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле 'Email' обов'язкове для заповнення")]
        [EmailAddress(ErrorMessage = "Некоректний формат Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле 'Рік народження' обов'язкове для заповнення")]
        [Range(1900, 2026, ErrorMessage = "Рік народження не може мати такого значення")]
        [Display(Name = "Рік народження")]
        public int? Year { get; set; }

        [Required(ErrorMessage = "Поле 'Пароль' обов'язкове для заповнення")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле 'Підтвердження паролю' обов'язкове")]
        [Compare("Password", ErrorMessage = "Паролі не збігаються")]
        [DataType(DataType.Password)]
        [Display(Name = "Підтвердження паролю")]
        public string PasswordConfirm { get; set; }
    }
}