using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElectBill.Models;

namespace ElectBill.Models
{
    public class BillDetail
    {
        public int Id { get; set; }
        public string Cust_Name { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public int Total_Amount { get; set;}
        public List<Items> Items { get; set; }

        public BillDetail() { 
            Items = new List<Items>();
        }
    }
}