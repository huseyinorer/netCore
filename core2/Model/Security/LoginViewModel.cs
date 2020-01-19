using System.ComponentModel.DataAnnotations;

namespace core2.Model.Security
{
    public class LoginViewModel//ders 63
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
