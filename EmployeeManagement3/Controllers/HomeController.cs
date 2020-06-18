using EmployeeManagement3.Models;
using EmployeeManagement3.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;
using System.Threading.Tasks;

namespace EmployeeManagement3.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GridDisplay()
        {
            var model = await _employeeRepository.GetAllEmployeesAsync();
            if (model == null) return NotFound();
            return View(model);         
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _employeeRepository.GetAllEmployeesAsync();
            if (model == null) return NotFound();
            return View(model);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            Employee employee = await _employeeRepository.GetEmployeeAsync(id.Value);

            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = employee,
                PageTitle = "Employee Details"
            };

            return View(homeDetailsViewModel);
        }


     //   [Authorize(Roles="Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (employee == null) return NotFound();

            if (ModelState.IsValid)
            {
                Employee newEmployee = await _employeeRepository.AddAsync(employee);
                return RedirectToAction("details", new { id = newEmployee.Id });
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            Employee employee = await _employeeRepository.GetEmployeeAsync(id.Value);

            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }

            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
            };

            return View(employeeEditViewModel);
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var model = await _employeeRepository.DeleteAsync(id.Value);

            if (model == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }

            return RedirectToAction("index");            
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = await _employeeRepository.GetEmployeeAsync(model.Id);

                // Update the employee object with the data in the model object
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;

                // Call update method on the repository service passing it the
                // employee object to update the data in the database table
                Employee updatedEmployee = await _employeeRepository.UpdateAsync(employee);
                return RedirectToAction("index");
            }

            return View(model);
        }

        #region stock methods

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion stock methods
    }
}