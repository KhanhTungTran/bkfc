
using System.Collections.Generic;


namespace bkfc.Models
{
    public static class Cart
    {
        public static List<Item> cart{get;set;}
    }
    public class Item
    {
        public Food food {get; set;}
        public int quantity{get; set;}
    }
}