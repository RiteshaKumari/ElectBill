using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElectBill.Models
{
    public class Login
    {



        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Required(ErrorMessage = "Please Enter Your Email")]
        // [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Your Password")]
        //[Compare("Password")]
        public string Pass { get; set; }

        [Display(Name = "Remember Me?")]
        public bool remember { get; set; }
    }
}