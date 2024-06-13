using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElectBill.Models
{
    public class Customer
    {
        public string OID { get; set; }
        public int cust_Id { get; set; }
        [Required]
        public string cust_Name { get; set; }
        [Required]
        public string cust_Mobile { get; set; }
        [Required]
        public string cust_Address { get; set; }
        public string password {  get; set; }

        public DateTime DateTime { get; set; }
        public string status { get; set; }
     
        public float? totalPrice { get; set; }
        public List<ItemDetail> Items { get; set; } = new List<ItemDetail>();

        
    }

}