using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace core2.Pages.Customers
{
    public class IndexModel : PageModel
    {
        public string message { get; set; }
        public void OnGet()
        {
            message += "Date is" + DateTime.Now.ToString();
        }
    }
}