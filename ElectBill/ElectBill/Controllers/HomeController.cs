﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ElectBill.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [Route("loginSignup")]
        public ActionResult loginSignup()
        {
            return View();
        }

        [Route("Welcome")]
        public ActionResult Welcome()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
  
    }
}