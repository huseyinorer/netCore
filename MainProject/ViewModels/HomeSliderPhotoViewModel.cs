using System.ComponentModel.DataAnnotations;

namespace MainProject.ViewModels
{
    public class HomeSliderPhotoViewModel
    {
        [Required(ErrorMessage = "Bir foto. seçiniz.")]
        [Display(Name = "Eklenecek Foto")]
        public byte[] Photo { get; set; }

        [Required(ErrorMessage = "Fotoğrafa bir başlık giriniz.")]
        [Display(Name = "Başlık")]
        public string PhotoTitle { get; set; }
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
    }
}
