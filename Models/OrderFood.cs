using System;
using System.ComponentModel.DataAnnotations;

namespace bkfc.Models
{
    public class OrderFood
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int FoodId { get; set; }
        public Food Food { get; set; }
    }
}