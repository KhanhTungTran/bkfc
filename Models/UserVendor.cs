using bkfc.Areas.Identity.Data;

namespace bkfc.Models
{
    public class UserVendor
    {
        public int UserId { get; set; }
        public bkfcUser User { get; set; }
        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }
    }
}