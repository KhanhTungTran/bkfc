using System;
using System.ComponentModel.DataAnnotations;

namespace bkfc.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double BalanceCharge { get; set; }
        public string FoodList { get; set; }
        public int UserId { get; set; }
        // public int OrderId { get; set; }
    }
}