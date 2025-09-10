namespace JakePOSApi.Models.Api
{
    public class RegisterRequestModel
    {
        public required string StoreName { get; set; }
        public required string StoreUsername { get; set; }

        public required string Password { get; set; }
        public required string StoreEmail { get; set; }
        public required string StorePhone { get; set; }

        public required string StoreOwnerName { get; set; }
    }
}
