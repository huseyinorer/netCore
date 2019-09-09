using System.ComponentModel.DataAnnotations;

namespace MainProject.Models
{
    public class Settings
    {
        [Key]
        public string WebSiteName { get; set; }

        public string Credentials_Email { get; set; }
        public string Credentials_Password { get; set; }
    }
}