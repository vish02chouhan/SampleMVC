using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.General.Interfaces
{
    interface IEmployeeRepository
    {
        IEnumerable<Employees> GetUsers();

        IEnumerable<Employees> GetEmployeesByQuery(string query);
    }
}
