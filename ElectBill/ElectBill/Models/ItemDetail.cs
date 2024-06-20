using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElectBill.Models
{
    public class ItemDetail
    {
        public int Item_Id { get; set; }
       
      
        public string Item_Name { get; set; }
        public string OID { get; set; }
      
        public int Item_Price { get; set; }
        public int Quantity { get; set; }
       public float? ToatalPrice {  get; set; }
       public int customerId { get; set; }
      
      
    }
}