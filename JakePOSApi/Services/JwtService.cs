using JakePOSApi.Data;
using JakePOSApi.Models.Api;
using JakePOSApi.Models.Entities;
using Microsoft.AspNetCore.Identity;
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
        private readonly PasswordHasher<StoreAccount> _hasher;

        public JwtService(ApplicationDbContext dBContext, IConfiguration configuration)
        {
            _dbContext = dBContext;
            _configuration = configuration;
            _hasher = new PasswordHasher<StoreAccount>();
        }

        public async Task<LoginResponseModel?> Authenticate(LoginRequestModel loginRequest)
        {
            if (string.IsNullOrWhiteSpace(loginRequest.UserName) || string.IsNullOrWhiteSpace(loginRequest.Password))
                return null;

            var storeAccount = _dbContext.StoreAccounts.FirstOrDefault(x => x.StoreUsername == loginRequest.UserName);

            if (storeAccount is null)
                return null;

            var result = _hasher.VerifyHashedPassword(storeAccount, storeAccount.StoreHashedPassword, loginRequest.Password);

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
