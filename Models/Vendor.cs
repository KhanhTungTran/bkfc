using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using bkfc.Areas.Identity.Data;

namespace bkfc.Models
{
    public class Vendor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Category { get; set; }
    }
}