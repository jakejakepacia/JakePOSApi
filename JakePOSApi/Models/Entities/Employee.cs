using JakePOSApi.Enums;

namespace JakePOSApi.Models.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Username { get; set; }
        public string HashedPassword { get; set; }
        public required string Email { get; set; }
        public int StoreAccountId { get; set; }

        public Role Role { get; set; }
    }
}
