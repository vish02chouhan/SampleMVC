using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Security
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