using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using core2.Entities;

namespace core2.Model
{
    public class EmployeeAddViewModel
    {
        public Employee Employee { get; set; }
        public List<SelectListItem> Cities { get;  set; }
    }
}