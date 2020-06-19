using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Security
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLEmployeeRepository(AppDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> DeleteAsync(int Id)
        {
            Employee employee = await _dbContext.Employees.FindAsync(Id);
            if (employee != null)
            {
                _dbContext.Employees.Remove(employee);
                await _dbContext.SaveChangesAsync();
            }
            return employee;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return _dbContext.Employees.OrderBy(e => e.Name);
        }

        public async Task<Employee> GetEmployeeAsync(int Id)
        {
            return await _dbContext.Employees.FindAsync(Id);
        }

        public async Task<Employee> UpdateAsync(Employee employeeChanges)
        {
            var employee = _dbContext.Employees.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return employeeChanges;
        }
    }
}