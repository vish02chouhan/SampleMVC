using Dapper;
using Domain.Entities;
using Infrastructure.General.Implementation.Helpers;
using Infrastructure.General.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.General.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public IBaseRepository<Employees> _employee { get; set; }
        private IDapperContext _context { get; set; }
        public EmployeeRepository(IBaseRepository<Employees> employee, IDapperContext context)
        {
            _employee = employee;
            _context = context;
        }

        public IEnumerable<Employees> GetUsers()
        {
            // in reality you would connet to Database from this point, but for demo purposes
            // we will just return in-memory data
            IEnumerable<Employees> employee = _employee.GetAll(StoredProcedureMap.GetEmployee);

            return employee;
        }

        public IEnumerable<Employees> GetEmployeesByQuery(string query)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@searchTerm", query);

            IEnumerable<Employees> employee = _context.Connection.Query<Employees>(StoredProcedureMap.GetPartnersByName, parameter, null, false, null, System.Data.CommandType.StoredProcedure);

            return employee;
        }
    }
}
