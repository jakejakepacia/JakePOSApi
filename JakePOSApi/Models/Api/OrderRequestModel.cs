namespace JakePOSApi.Models.Api
{
    public class OrderRequestModel
    {
        public required List<int> ProductIds { get; set; }
        public decimal TotalAmount { get; set; }
        public int StoreId { get; set; }
        public int EmployeeId { get; set; }
    }
}
