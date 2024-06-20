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
        //[Required]
        [Required(ErrorMessage = "Customer name is required")]
        [StringLength(maximumLength: 30, MinimumLength = 4, ErrorMessage = "max length is 20 and min length is 4")]
        public string cust_Name { get; set; }
       // [Required]
       [Required(ErrorMessage = "Mobile is required")]
        [RegularExpression("^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string cust_Mobile { get; set; }

        public string password {  get; set; }

        public DateTime DateTime { get; set; }
        public string status { get; set; }
     
        public float? totalPrice { get; set; }
        public List<ItemDetail> Items { get; set; } = new List<ItemDetail>();

        public int ProductId { get; set; }
        public string ProductName { get; set; } 

        public int ProductPrice { get; set; }
        public string Date { get; set; }
    }

}