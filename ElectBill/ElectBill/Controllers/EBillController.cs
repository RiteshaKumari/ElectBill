using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectBill.Controllers
{
    public class EBillController : Controller
    {
        // GET: EBill
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateBill()
        {
            return View();
        }
    }
}