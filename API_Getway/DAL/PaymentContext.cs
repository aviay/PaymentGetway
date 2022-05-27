using API_Getway.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Getway.DAL
{
    public class PaymentContext : DbContext
    {
        public PaymentContext(DbContextOptions<PaymentContext> options)
            : base(options)
        {
        }

        public DbSet<Merchant> Merchants { get; set; }

        public DbSet<Charge> Charges { get; set; }
    }
}
