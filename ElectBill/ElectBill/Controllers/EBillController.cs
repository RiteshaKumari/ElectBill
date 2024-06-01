using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectBill.Models;
using ElectBill.Repository;


namespace ElectBill.Controllers
{
    public class EBillController : Controller
    {
        // GET: EBill
        public ActionResult Index() // view data per y dikta per usme view per click then per go to ViewBill
        {
            Data data = new Data();
            var list = data.GetAllDetails();
            return View(list);
        }
        public ActionResult CreateBill()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateBill(BillDetail  Details)
        {
            Data data = new Data();
            data.SaveBillDetails(Details);
            ModelState.Clear();
           return View();
        }

       
        public ActionResult CreateItem(Items item)
        {
            return PartialView("_CreateItem", item);
        }

        public ActionResult ViewBill(int id)
        {
            Data data = new Data();
           var details = data.GetoneBillDetail(id);
            return View(details);
        }
    }
}