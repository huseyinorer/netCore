using System.ComponentModel.DataAnnotations;

namespace MainProject.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Display(Name = "Tel. No:")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email adresiniz gereklidir.")]
        [Display(Name = "Email adresiniz")]
        [EmailAddress(ErrorMessage = "Email adresi doğru formatta değil.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifreniz gereklidir.")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}