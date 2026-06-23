using Microsoft.EntityFrameworkCore;

namespace BankAPI.Entities
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions contextOptions) : base(contextOptions) { }

        public DbSet<User> Users { get; set; }
    }
}
