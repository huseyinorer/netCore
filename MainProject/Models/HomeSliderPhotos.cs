using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainProject.Models
{
    public class HomeSliderPhotos
    {   [Key]
        public int PhotoId { get; set; }
        public byte[] Photo { get; set; }
        public DateTime PhotoAddDate { get; set; }
        public string PhotoTitle { get; set; }
        public string Description { get; set; }
    }
}
