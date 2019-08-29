using System.ComponentModel.DataAnnotations;

namespace MainProject.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "Eski Şifre")]
        [Required]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Şifre En Az 4 Karakter İçermeli!")]
        public string OldPassword { get; set; }

        [Display(Name = "Yeni Şifre")]
        [Required]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Şifre En Az 4 Karakter İçermeli!")]
        public string NewPassword { get; set; }

        [Display(Name = "Yeni Şifre Onayla")]
        [Required]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Şifre En Az 4 Karakter İçermeli!")]
        [Compare("NewPassword", ErrorMessage = "Şifre Uyuşmamaktadır!")]
        public string ConfirmPassword { get; set; }
    }
}