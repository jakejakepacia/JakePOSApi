using JakePOSApi.Data;
using JakePOSApi.Models.Api;
using JakePOSApi.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace JakePOSApi.Services
{
    public class StoreAccountService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly PasswordHasher<StoreAccount> _passwordHasher;

        public StoreAccountService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _passwordHasher = new PasswordHasher<StoreAccount>();
            
        }

        public OperationResult RegisterStoreAccount(RegisterRequestModel model)
        {
            var userExist = CheckUsernameIfTaken(model.StoreUsername);
            if (userExist)
                return OperationResult.FailureResult(new List<string> { "Username already taken" });

            var newStoreAccount = new StoreAccount
            {
               StoreName = model.StoreName,
               StoreEmail = model.StoreEmail,
               StoreUsername = model.StoreUsername,
               StoreOwnerName = model.StoreOwnerName,
               StorePhone = model.StorePhone,
            };

            newStoreAccount.StoreHashedPassword = _passwordHasher.HashPassword(newStoreAccount, model.Password);


            _dbContext.StoreAccounts.Add(newStoreAccount);
            _dbContext.SaveChanges();

         
            return OperationResult.SuccessResult("User registered successfully");
        }

        private bool CheckUsernameIfTaken(string username)
        {
            var user = _dbContext.StoreAccounts.FirstOrDefault(u => u.StoreUsername == username);

            return user != null;
        }
    }
}
