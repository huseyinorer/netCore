using MainProject.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace MainProject.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [RegularExpression(@"^(0(\d{3}) (\d{3}) (\d{2}) (\d{2}))$", ErrorMessage = "Telefon numarası uygun formatta değil")]
        [Display(Name = "Tel. No:")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email adresiniz gereklidir.")]
        [Display(Name = "Email adresiniz")]
        [EmailAddress(ErrorMessage = "Email adresi doğru formatta değil.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifreniz gereklidir.")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Şehir")]
        public string City { get; set; }
        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        [Display(Name = "Cinsiyet")]
        public Gender Gender { get; set; }

    }
}