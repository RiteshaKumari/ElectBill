using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectBill.Models
{
    public class Items
    {
        public int Id { get; set; }
        public string Prod_Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int Item_Index { get; set; }
    }
}