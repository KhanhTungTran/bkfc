using Microsoft.EntityFrameworkCore;
using bkfc.Models;

namespace bkfc.Data
{
    public class bkfcContext : DbContext
    {
        public bkfcContext(DbContextOptions<bkfcContext> options) : base(options)
        {
        }

        public DbSet<Vendor> Vendor { get; set; }
        public DbSet<Food> Food { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Report> Report { get; set; }
    }
}