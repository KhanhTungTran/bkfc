using System;
using System.ComponentModel.DataAnnotations;

namespace bkfc.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string FoodSold { get; set; }
        public double Income { get; set; }
        public int VendorId { get; set; }
    }
}