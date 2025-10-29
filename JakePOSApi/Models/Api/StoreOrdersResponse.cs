using JakePOSApi.Models.Entities;

namespace JakePOSApi.Models.Api
{
    public class StoreOrdersResponse
    {
        public int Id { get; set; }
        public required List<StoreProduct> Products { get; set; }
        public decimal TotalAmount { get; set; }
        public int StoreId { get; set; }
        public int EmployeeId { get; set; }
        public bool IsProcessedByOwner { get; set; }
        public DateTime OrderDateTime { get; set; }
    }
}
