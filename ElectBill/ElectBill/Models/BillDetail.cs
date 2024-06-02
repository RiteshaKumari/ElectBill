using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ElectBill.Models;

namespace ElectBill.Models
{
    public class BillDetail
    {
        public int Id { get; set; }
        [Required]
        public string Cust_Name { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int Total_Amount { get; set;}

        public List<Items> Items { get; set; }

        public BillDetail() { 
            Items = new List<Items>();
        }
    }
}