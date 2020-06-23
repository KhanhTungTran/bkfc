using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace bkfc.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int PaymentId { get; set; }
        public int VendorId { get; set; }
        public IList<OrderFood> OrderFoods { get; set; }
    }
}