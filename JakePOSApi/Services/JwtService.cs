using JakePOSApi.Data;
using JakePOSApi.Models.Api;
using JakePOSApi.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JakePOSApi.Services
{
    public class JwtService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<StoreAccount> _storePasswordHasher;
        private readonly PasswordHasher<Employee> _employeePasswordHasher;

        public JwtService(ApplicationDbContext dBContext, IConfiguration configuration)
        {
            _dbContext = dBContext;
            _configuration = configuration;
            _storePasswordHasher = new PasswordHasher<StoreAccount>();
            _employeePasswordHasher = new PasswordHasher<Employee>();
        }

        private async Task<LoginResponseModel?> AuthenticateEmployee(LoginRequestModel loginRequest)
        {
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Username == loginRequest.UserName);

            if (employee is null)
                return null;

            var result = _employeePasswordHasher.VerifyHashedPassword(employee, employee.HashedPassword, loginRequest.Password);

            if (result == PasswordVerificationResult.Failed)
                return null;

            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var key = _configuration["JwtSettings:SecretKey"];
            var tokenValidityMins = _configuration.GetValue<int>("JwtSettings:ExpiresInMinutes");
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                  new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                  new Claim(ClaimTypes.Name, employee.Username),
                  new Claim(ClaimTypes.Role, employee.Role.ToString())
                }),

                Expires = tokenExpiryTimeStamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return new LoginResponseModel
            {
                Id = employee.Id,
                AccessToken = accessToken,
                Username = loginRequest.UserName,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
            };
        }


      
        public async Task<LoginResponseModel?> Authenticate(LoginRequestModel loginRequest)
        {
            if (string.IsNullOrWhiteSpace(loginRequest.UserName) || string.IsNullOrWhiteSpace(loginRequest.Password))
                return null;

            var storeAccount = _dbContext.StoreAccounts.FirstOrDefault(x => x.StoreUsername == loginRequest.UserName);

            if (storeAccount is null)
            {
                return await AuthenticateEmployee(loginRequest);
            }

            var result = _storePasswordHasher.VerifyHashedPassword(storeAccount, storeAccount.StoreHashedPassword, loginRequest.Password);

            if (result == PasswordVerificationResult.Failed)
                return null;

            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var key = _configuration["JwtSettings:SecretKey"];
            var tokenValidityMins = _configuration.GetValue<int>("JwtSettings:ExpiresInMinutes");
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                  new Claim(ClaimTypes.NameIdentifier, storeAccount.Id.ToString()),
                  new Claim(ClaimTypes.Name, storeAccount.StoreUsername),
                  new Claim(ClaimTypes.Role, "StoreManager")
                }),

                Expires = tokenExpiryTimeStamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return new LoginResponseModel
            {
                Id = storeAccount.Id,
                AccessToken = accessToken,
                Username = loginRequest.UserName,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
            };

        }
    }
}
