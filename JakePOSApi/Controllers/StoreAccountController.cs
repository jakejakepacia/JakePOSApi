using JakePOSApi.Data;
using JakePOSApi.Models.Api;
using JakePOSApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JakePOSApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreAccountController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly StoreAccountService _storeAccountService;
        public StoreAccountController(ApplicationDbContext dbContext, StoreAccountService storeAccountService)
        {
            _dbContext = dbContext;
            _storeAccountService = storeAccountService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterRequestModel request)
        {
            var result = _storeAccountService.RegisterStoreAccount(request);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(result.Message);
        }
    }
  }
