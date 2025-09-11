namespace JakePOSApi.Models.Api
{
    public class ProductRequestModel
    {
        public required string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StoreId { get; set; }
        public int EmployeeId { get; set; }
        public bool IsAddedByOwner { get; set; }
    }
}
