using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace bkfc.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double BalanceCharge { get; set; }
        public int UserId { get; set; }
        public IList<PaymentFood> PaymentFoods { get; set; }
    }
}