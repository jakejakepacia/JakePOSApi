namespace JakePOSApi.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public required List<int > ProductIds { get; set; }
        public decimal TotalAmount { get; set; }
        public int StoreId { get; set; }
        public int EmployeeId { get; set; }
        public bool IsProcessedByOwner { get; set; }
        public DateTime OrderDateTime { get; set; }

    }
}
