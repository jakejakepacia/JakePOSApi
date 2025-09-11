using JakePOSApi.Data;
using JakePOSApi.Models.Api;
using JakePOSApi.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace JakePOSApi.Services
{
    public class EmployeeService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly PasswordHasher<Employee> _passwordHasher;

        public EmployeeService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _passwordHasher = new PasswordHasher<Employee>();
        }

        public async Task<OperationResult> AddEmployee(EmployeeRequestModel model)
        {
            if (CheckUsernameIfTaken(model.Username))
            {
                return OperationResult.FailureResult(["Username is taken"]);
            }

            var newEmployee = new Employee { 
                Email = model.Email,
                Username = model.Username,
                StoreAccountId = model.StoreId,
                Name = model.Name,
                Role = model.Role
            };

             newEmployee.HashedPassword = _passwordHasher.HashPassword(newEmployee, model.Password);

            await _dbContext.Employees.AddAsync(newEmployee);
            await _dbContext.SaveChangesAsync();

            return OperationResult.SuccessResult();

        }

        private bool CheckUsernameIfTaken(string username)
        {
            var user = _dbContext.StoreAccounts.FirstOrDefault(u => u.StoreUsername == username);
                var employee = _dbContext.Employees.FirstOrDefault(e => e.Username == username);

            return user != null && employee != null;
        }
    }
}
