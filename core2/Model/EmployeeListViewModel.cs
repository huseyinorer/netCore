using core2.Entities;
using System.Collections.Generic;

namespace core2.Model
{
    public class EmployeeListViewModel
    {
        public List<Employee> Employees { get; set; }
        public List<string> Cities { get; set; }
    }
}