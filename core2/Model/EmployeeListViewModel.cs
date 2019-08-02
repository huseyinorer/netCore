using System.Collections.Generic;
using core2.Entities;

namespace core2.Model
{
    public class EmployeeListViewModel
    {
        public List<Employee> Employees { get; set; }
        public List<string> Cities { get; set; }
    }
}