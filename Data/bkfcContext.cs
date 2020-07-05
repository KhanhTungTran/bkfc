using Microsoft.EntityFrameworkCore;
using bkfc.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace bkfc.Data
{
    public class bkfcContext : DbContext
    {
        public bkfcContext(DbContextOptions<bkfcContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PaymentFood>().HasKey(pf => new { pf.PaymentId, pf.FoodId, });
            builder.Entity<OrderFood>().HasKey(of => new { of.OrderId, of.FoodId, });
            builder.Entity<UserVendor>().HasKey(of => new { of.UserId, of.VendorId });
        }

        public DbSet<Vendor> Vendor { get; set; }
        public DbSet<Food> Food { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<PaymentFood> PaymentFoods { get; set; }
        public DbSet<OrderFood> OrderFoods { get; set; }
        public DbSet<UserVendor> UserVendors { get; set; }
    }
}