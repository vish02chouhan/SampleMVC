using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagement3.Models
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetEmployeeAsync(int Id);

        Task<IEnumerable<Employee>> GetAllEmployeesAsync();

        Task<Employee> AddAsync(Employee employee);

        Task<Employee> UpdateAsync(Employee employeeChanges);

        Task<Employee> DeleteAsync(int Id);
    }
}