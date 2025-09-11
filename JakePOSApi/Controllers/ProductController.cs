using JakePOSApi.Enums;
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
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = $"{nameof(Role.StoreOwner)},{nameof(Role.Administrator)}")]
        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct(ProductRequestModel model)
        {
            var result = await _productService.AddProduct(model);

            return Ok(result);
        }

        [HttpGet("{storeId}")]
        public async Task<IActionResult> GetAllProducts(int storeId)
        {
            var result = await _productService.GetAllProductsByStoreId(storeId);

            return Ok(result);
        }
    }
}
