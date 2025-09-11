using JakePOSApi.Enums;

namespace JakePOSApi.Models.Api
{
    public class EmployeeRequestModel
    {
        public required string Name { get; set; }
        public required string Username { get; set; }
        public string Password { get; set; }
        public required string Email { get; set; }
        public int StoreId { get; set; }
        public Role Role { get; set; }
    }
}

