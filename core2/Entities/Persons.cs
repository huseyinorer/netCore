using System.ComponentModel.DataAnnotations;

namespace core2.Entities
{
    public class persons
    {
        [Key]
        public int personid { get; set; }
        public string lastname { get; set; }
        public string firstname { get; set; }
        public string address { get; set; }
        public string city { get; set; }
    }
}
