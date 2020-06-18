using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement3.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
    }
}
