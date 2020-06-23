
using System.Collections.Generic;
// using Newtonsoft.Json;


namespace bkfc.Models
{
    public class Item
    {
        public Food food {get; set;}
        public int quantity{get; set;}
        
        // public List<Item> deserialize(object str)
        // {
        //     return JsonConvert.DeserializeObject<List<Item>>(str as string);
        // }

        // public string serialize(object obj)
        // {
        //     return JsonConvert.SerializeObject(obj);
        // }
    }

}