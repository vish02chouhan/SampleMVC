using System;

namespace Domain.Entities
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string ConnectionString { get; set; }
        public string ConnectionStringPassword { get; set; }
        public string Password { get; set; }
        public string EmployeeImageUrl { get; set; }
        public string AllowOrigin { get; set; }
        public int RegistrationFrequencyDays { get; set; }
        public byte RegistrationsReportMonths { get; set; }
        public byte RegistrationsMonths { get; set; }
    }
}
