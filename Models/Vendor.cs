using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bkfc.Models
{
    public class Vendor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Category { get; set; }
        public IList<UserVendor> UserVendor { get; set; }
    }
}