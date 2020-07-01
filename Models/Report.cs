using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Collections.Generic;
namespace bkfc.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string FoodSold { get;set;}

        public double Income { get; set; }
        public int VendorId { get; set; }
        // public IList<Order> ListOrder
        // { 
        //     get{
        //         return JsonConvert.DeserializeObject<IList<Order>(FoodSold);
        //     ;} 
        //     set{
        //         FoodSold = JsonConvert.SerializeObject(value);
        //     } 
        // }
    }
}