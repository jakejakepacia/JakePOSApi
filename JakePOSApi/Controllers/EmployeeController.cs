using JakePOSApi.Models.Api;
using JakePOSApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JakePOSApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;
        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        [Route("addEmployee")]
        public async Task<IActionResult> AddEmployee(EmployeeRequestModel model)
        {
            var result = await _employeeService.AddEmployee(model);

            return Ok(result);
        }

    }
}
