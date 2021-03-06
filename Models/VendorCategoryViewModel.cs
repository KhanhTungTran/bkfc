using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace bkfc.Models
{
    public class VendorCategoryViewModel
    {
        public List<Vendor> Vendors { get; set; }
        public SelectList Category { get; set; }
        public string VendorCategory { get; set; }
        public string SearchString { get; set; }
    }
}