//using Domain.Entities;
//using System.Collections.Generic;
//using System.Linq;

//namespace Infrastructure.Security
//{
//    public class MockEmployeeRepository : IEmployeeRepository
//    {
//        private readonly List<Employee> _employeeList;

//        public MockEmployeeRepository()
//        {
//            _employeeList = new List<Employee>()
//            {
//                new Employee() { Id = 101, Name = "Mike", Department = Dept.IT, Email = "mike@mail.com" },
//                new Employee() { Id = 102, Name = "Steve", Department = Dept.IT, Email = "steve@mail.com" },
//                new Employee() { Id = 103, Name = "Gwen", Department = Dept.IT, Email = "gwen@mail.com" },
//            };
//        }

//        public Employee Add(Employee employee)
//        {
//            employee.Id = _employeeList.Max(e => e.Id) + 1;
//            _employeeList.Add(employee);
//            return employee;
//        }

//        public Employee Delete(int Id)
//        {
//            Employee employee = _employeeList.FirstOrDefault(e => e.Id == Id);
//            if (employee != null)
//            {
//                _employeeList.Remove(employee);
//            }
//            return employee;
//        }

//        public IEnumerable<Employee> GetAllEmployees()
//        {
//            return _employeeList;
//        }

//        public Employee GetEmployee(int Id)
//        {
//            return this._employeeList.FirstOrDefault(e => e.Id == Id);
//        }

//        public Employee Update(Employee employeeChanges)
//        {
//            Employee employee = _employeeList.FirstOrDefault(e => e.Id == employeeChanges.Id);
//            if (employee != null)
//            {
//                employee.Name = employeeChanges.Name;
//                employee.Email = employeeChanges.Email;
//                employee.Department = employeeChanges.Department;
//            }
//            return employee;
//        }
//    }
//}