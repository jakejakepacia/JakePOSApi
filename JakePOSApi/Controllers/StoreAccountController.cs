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
        private readonly StoreAccountService _storeAccountService;
        private readonly JwtService _jwtService;
        public StoreAccountController(JwtService jwtService, StoreAccountService storeAccountService)
        {
            _storeAccountService = storeAccountService;
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel request)
        {
            var result = await _jwtService.Authenticate(request);

            if (result == null)
                return Unauthorized();

            return result;
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
