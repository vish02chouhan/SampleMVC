using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement3.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
