using JakePOSApi.Data;
using JakePOSApi.Models.Api;
using JakePOSApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;

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
                TotalAmount = orderRequestModel.TotalAmount
            };

            _dbContext.StoreOrders.Add(newOrder);
            _dbContext.SaveChanges();

            return newOrder.Id;
        }

        public async Task<List<StoreOrdersResponse>> GetOrdersAsync(int storeId)
        {
            // 1️⃣ Get all orders for this store
            var orders = await _dbContext.StoreOrders
                .Where(s => s.StoreId == storeId)
                .ToListAsync();

            if (!orders.Any())
                return new List<StoreOrdersResponse>();

            // 2️⃣ Collect all product IDs across all orders
            var allProductIds = orders
                .SelectMany(o => o.ProductIds)
                .Distinct()
                .ToList();

            // 3️⃣ Fetch all products in one go
            var products = await _dbContext.StoreProducts
                .Where(p => allProductIds.Contains(p.Id))
                .ToListAsync();

            // Create a dictionary for faster lookups
            var productMap = products.ToDictionary(p => p.Id);

            // 4️⃣ Build responses
            var storeOrders = new List<StoreOrdersResponse>();

            foreach (var order in orders)
            {
                var productsForOrder = new List<StoreProduct>();

                foreach (var productId in order.ProductIds)
                {
                    if (productMap.TryGetValue(productId, out var product))
                    {
                        productsForOrder.Add(product);
                    }
                }

                var storeOrder = new StoreOrdersResponse
                {
                    Id = order.Id,
                    Products = productsForOrder,
                    TotalAmount = order.TotalAmount,
                    StoreId = order.StoreId,
                    IsProcessedByOwner = order.IsProcessedByOwner,
                    OrderDateTime = order.OrderDateTime
                };

                storeOrders.Add(storeOrder);
            }

            return storeOrders;
        }

        public async Task<List<StoreOrdersResponse>> GetOrdersByDateAsync(int storeId, DateTime dateTime)
        {
            // 1️⃣ Get all orders for this store
            var orders = await _dbContext.StoreOrders
                .Where(s => s.StoreId == storeId && s.OrderDateTime.Date ==  dateTime.Date)
                .ToListAsync();

            if (!orders.Any())
                return new List<StoreOrdersResponse>();

            // 2️⃣ Collect all product IDs across all orders
            var allProductIds = orders
                .SelectMany(o => o.ProductIds)
                .Distinct()
                .ToList();

            // 3️⃣ Fetch all products in one go
            var products = await _dbContext.StoreProducts
                .Where(p => allProductIds.Contains(p.Id))
                .ToListAsync();

            // Create a dictionary for faster lookups
            var productMap = products.ToDictionary(p => p.Id);

            // 4️⃣ Build responses
            var storeOrders = new List<StoreOrdersResponse>();

            foreach (var order in orders)
            {
                var productsForOrder = new List<StoreProduct>();

                foreach (var productId in order.ProductIds)
                {
                    if (productMap.TryGetValue(productId, out var product))
                    {
                        productsForOrder.Add(product);
                    }
                }

                var storeOrder = new StoreOrdersResponse
                {
                    Id = order.Id,
                    Products = productsForOrder,
                    TotalAmount = order.TotalAmount,
                    StoreId = order.StoreId,
                    IsProcessedByOwner = order.IsProcessedByOwner,
                    OrderDateTime = order.OrderDateTime
                };

                storeOrders.Add(storeOrder);
            }

            return storeOrders;
        }

    }
}
