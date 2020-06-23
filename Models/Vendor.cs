using System;
using System.ComponentModel.DataAnnotations;

namespace bkfc.Models
{
    public class Vendor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        [Display(Name = "List of Food and Drinks")]
        // public string FoodList { get; set; }
        public string Category { get; set; }
    }
}