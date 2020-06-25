using System;
using System.ComponentModel.DataAnnotations;

namespace bkfc.Models
{
    public class PaymentFood
    {
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
        public int FoodId { get; set; }
        public Food Food { get; set; }
        public int Amount { get; set; }
    }
}