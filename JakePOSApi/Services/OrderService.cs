using JakePOSApi.Data;
using JakePOSApi.Models.Api;
using JakePOSApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JakePOSApi.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _dbContext;
        public OrderService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;   
        }

        public int AddOrder(OrderRequestModel orderRequestModel)
        {
            var newOrder = new Order
            {
                ProductIds = orderRequestModel.ProductIds,
                StoreId = orderRequestModel.StoreId,
                EmployeeId  = orderRequestModel.EmployeeId,
                IsProcessedByOwner = orderRequestModel.EmployeeId == 0,
                OrderDateTime = DateTime.UtcNow,
            };

            _dbContext.StoreOrders.Add(newOrder);
            _dbContext.SaveChanges();

            return newOrder.Id;
        }

        public async Task<List<Order>> GetOrdersAsync(int storeId)
        {
            var orders = await _dbContext.StoreOrders.Where(s => s.StoreId == storeId).ToListAsync();

            return orders;
        }
    }
}
