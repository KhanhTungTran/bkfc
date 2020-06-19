using System;
using System.ComponentModel.DataAnnotations;

namespace bkfc.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        [Display(Name = "List of Food and Drinks")]
        public string FoodList { get; set; }
        public int PaymentId { get; set; }
        public int VendorId { get; set; }
    }
}