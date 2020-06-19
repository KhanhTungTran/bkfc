using System;
using System.ComponentModel.DataAnnotations;

namespace bkfc.Models
{
    public class Food
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public double Discount { get; set; }
    }
}