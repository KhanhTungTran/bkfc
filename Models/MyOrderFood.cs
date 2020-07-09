using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace bkfc.Models
{
    public class MyOrderFood
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public IList<Food> Foods { get; set; }
        public DateTime Date{get;set;}
        public int EstimateMinutes {get;set;}
    }
}