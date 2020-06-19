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
    }
}