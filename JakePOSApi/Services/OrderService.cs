using JakePOSApi.Data;
using JakePOSApi.Models.Api;
using JakePOSApi.Models.Entities;

namespace JakePOSApi.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _dbContext;
        public OrderService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;   
        }

        public OperationResult AddOrder(OrderRequestModel orderRequestModel)
        {
            var newOrder = new Order
            {
                ProductIds = orderRequestModel.ProductIds,
                StoreId = orderRequestModel.StoreId,
                EmployeeId  = orderRequestModel.EmployeeId,
                IsProcessedByOwner = orderRequestModel.IsProcessedByOwner,
                OrderDateTime = DateTime.Now,
            };

            _dbContext.StoreOrders.Add(newOrder);

            return OperationResult.SuccessResult("Order placed");
        }
    }
}
