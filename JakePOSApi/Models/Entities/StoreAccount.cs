namespace JakePOSApi.Models.Entities
{
    public class StoreAccount
    {
        public int Id { get; set; }
        public required string StoreName { get; set; }
        public required string StoreUsername { get; set; }

        public string StoreHashedPassword { get; set; }
        public required string StoreEmail { get; set; }
        public required string StorePhone { get; set; }

        public required string StoreOwnerName { get; set; }

    }
}
