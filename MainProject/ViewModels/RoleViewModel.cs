using System.ComponentModel.DataAnnotations;

namespace MainProject.ViewModels
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Rol ismi gereklidir.")]
        [Display(Name = "Rol İsmi")]
        public string Name { get; set; }

        public string Id { get; set; }
    }
}
