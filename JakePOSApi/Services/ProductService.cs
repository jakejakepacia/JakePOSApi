using JakePOSApi.Data;
using JakePOSApi.Models.Api;
using JakePOSApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JakePOSApi.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductService(ApplicationDbContext dbContext)
        {
             _dbContext = dbContext;   
        }

        public async Task<OperationResult> AddProduct(ProductRequestModel model)
        {
            var newProduct = new StoreProduct
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                StoreId = model.StoreId,
                EmployeeId = model.EmployeeId,
                IsAddedByOwner = model.IsAddedByOwner,
            };

            await _dbContext.StoreProducts.AddAsync(newProduct);
            await _dbContext.SaveChangesAsync();

            return OperationResult.SuccessResult("Product added");
        }

        public async Task<List<StoreProduct>?> GetAllProductsByStoreId(int storeId)
        {
            var products = await _dbContext.StoreProducts.Where(p => p.StoreId == storeId).ToListAsync();

            return products;
        }

    }
}
