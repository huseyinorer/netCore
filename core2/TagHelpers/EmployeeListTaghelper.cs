using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using core2.Entities;

namespace core2.TagHelpers
{
    [HtmlTargetElement("employee-list")]
    public class EmployeeListTaghelper:TagHelper
    {
        private List<Employee> _employees;
        public EmployeeListTaghelper()
        {
            _employees = new List<Employee>
            {
                new Employee{Id=1,FirstName="husam",LastName="orr",CityId=1},
                new Employee{Id=2,FirstName="husam2",LastName="orr2",CityId=6},
                new Employee{Id=3,FirstName="husam3",LastName="orr3",CityId=23},

            };

        }

        private const string ListCountAttributeName="count";

        [HtmlAttributeName(ListCountAttributeName)]
        public int ListCount { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            var query = _employees.Take(ListCount);
            StringBuilder strbuilding = new StringBuilder();

            foreach (var employee in query)
            {
                strbuilding.AppendFormat("<h2><a href='/employee/detail/{0}'>{1}</a></h2>",employee.Id,employee.FirstName);
            }
            output.Content.SetHtmlContent(strbuilding.ToString());
            base.Process(context, output);
        }
    }
}
