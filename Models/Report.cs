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
        private string FoodSold { get;set;}

        public List<OrderFood> ListFoodSold
        { 
            get{
                return JsonConvert.DeserializeObject<List<OrderFood>>(FoodSold);
            ;} 
            set{
                FoodSold = JsonConvert.SerializeObject(value);
            } 
        }
        public double Income { get; set; }
        public int VendorId { get; set; }
    }
}