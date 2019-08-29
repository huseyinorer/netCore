using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainProject.ViewModels
{
    public class LoginViewModel//ders 63
    {
        [Required(ErrorMessage ="Email Adresi Gereklidir")]
        [Display(Name ="Email Adresiniz")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifre Gereklidir")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]        
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
