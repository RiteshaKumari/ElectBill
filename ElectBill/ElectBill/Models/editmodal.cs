using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectBill.Models
{
    public class editmodal
    {

        public int OID { get; set; }
        public string Item_Name { get; set; }
        public string Item_Price { get; set; }
        public int Quantity { get; set; }
        public float totalPrice { get; set; }
       
    }
}