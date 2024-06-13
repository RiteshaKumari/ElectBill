using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectBill.Models
{
    public class Invoice_Detail
    {
     public int InvoDetailId { get; set; }
        public int InvoMasId{ get; set; }
        public int ItemId { get; set; }
        public float ItemPrice { get; set;}
        public string ItemName { get; set; }
        public int ItemQuantity { get; set; }
        public float totalPrice { get; set; }
    }
}