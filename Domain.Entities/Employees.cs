using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Employees
    {
        public Guid ContentId { get; set; }
        public string EmployeeCode { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FamiliarName { get; set; }
        public string Title { get; set; }
        public string OfficeName { get; set; }
        public string LevelGrade { get; set; }
        public string EmployeeStatusCode { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public string LastUpdatedName { get; set; }


    }
}
