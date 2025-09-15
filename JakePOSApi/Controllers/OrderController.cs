using JakePOSApi.Models.Api;
using JakePOSApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JakePOSApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        public OrderController(OrderService orderService)
        {
                _orderService = orderService;
        }

        [HttpPost]
        [Route("checkout")]
        public IActionResult AddOrder(OrderRequestModel model)
        {
            var result = _orderService.AddOrder(model);

            return Ok(result);

        }
    }
}
