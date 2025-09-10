using JakePOSApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JakePOSApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<StoreAccount> StoreAccounts { get; set; }
    }
}
