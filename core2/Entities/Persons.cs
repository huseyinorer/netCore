using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
