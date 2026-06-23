using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class BankDbContext : DbContext
    {
        public BankDbContext (DbContextOptions<BankDbContext> options) : base(contextOptions)
    }
}