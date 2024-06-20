
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Data;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using ElectBill.Models;
//using ElectBill.Repository;
//using System.Configuration;
//using System.Reflection;
//using System.Web.Security;


//namespace ElectBill.Controllers
//{
//    public class EBillController : Controller
//    {
//        // GET: EBill
//        Utility us = new Utility();


//private void loadbag()
//{

//    List<BillDetail> Res = new List<BillDetail>();

//    DataSet DS = us.fn_DataSet("dropdown");
//    var Book = DS.Tables[0];
//    var Res2 = Book.AsEnumerable().Select(s => new BillDetail
//    {
//        ProductId = s.Field<int>("ProductId"),
//        ProdName = s.Field<string>("ProdName")


//    }).ToList();
//    ViewBag.AllProduct = new SelectList(Res2, "ProductId", "ProdName");
//}

//        public string connectionString = ConfigurationManager.ConnectionStrings["mycon"].ConnectionString;

//private List<BillDetail> GetDropName()
//{
//    var items = new List<BillDetail>();
//    items.Add(new BillDetail() { ListId = 1, Title = "Customer Name" });
//    items.Add(new BillDetail() { ListId = 2, Title = "Mobile" });
//    items.Add(new BillDetail() { ListId = 3, Title = "Address" });
//    items.Add(new BillDetail() { ListId = 4, Title = "Total_Amount" });
//    items.Add(new BillDetail() { ListId = 5, Title = "dateTime" });
//    return items;
//}

//        [Authorize]
//        public ActionResult Index() // view data per y dikta per usme view per click then per go to ViewBill
//        {
//            Response.Cache.SetCacheability(HttpCacheability.NoCache);
//            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
//            Response.Cache.SetNoStore();

//            if (ModelState.IsValid)
//            {
//                if (Request.IsAuthenticated && Request.Cookies["Email"] != null)
//                {
//                    TempData["logindata"] = Request.Cookies["Email"].Value;
//                    var userEmail = Request.Cookies["Email"].Value;


//                    Response.Cookies["Email"].Value = userEmail;
//                }
//                ViewBag.SortTitle = new SelectList(GetDropName(), "ListId", "Title");
//                Data data = new Data();
//                var list = data.GetAllDetails();
//                return View(list);
//            }
//            TempData["alert"] = "All Fields are required !";

//            return View();
//        }

//        [HttpPost]
//        public ActionResult Index(BillDetail list)
//        {
//            SqlParameter[] parameters2 = new SqlParameter[]
//            {
//                new SqlParameter("@Id", list.ListId),
//            };


//            var valid = (int)us.func_ExecuteScalar("SortList", parameters2);


//            if (valid > 0)
//            {
//                ModelState.Clear();

//                Data data = new Data();
//                var listItem = data.GetAlllists();
//                ViewBag.SortTitle = new SelectList(GetDropName(), "ListId", "Title");
//                return View(listItem);
//            }
//            else
//            {
//                ViewBag.SortTitle = new SelectList(GetDropName(), "ListId", "Title");
//                TempData["listalert"] = "Something went wrong !";
//                return View();
//            }
//        }

//        [Route("CreateBill")]
//        [Authorize]
//        public ActionResult CreateBill()
//        {
//            Response.Cache.SetCacheability(HttpCacheability.NoCache);
//            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
//            Response.Cache.SetNoStore();

//            if (Request.IsAuthenticated && Request.Cookies["Email"] != null)
//            {
//                TempData["logindata"] = Request.Cookies["Email"].Value;
//                var userEmail = Request.Cookies["Email"].Value;


//                Response.Cookies["Email"].Value = userEmail;
//            }
//            //  loadbag();
//            return View();
//        }

//        [HttpPost]

//        [Route("CreateBill")]
//        [Authorize]
//        public ActionResult CreateBill(BillDetail Details)
//        {
//            if (ModelState.IsValid)
//            {
//                Data data = new Data();

//                TempData["detailviewID"] = data.SaveBillDetails(Details);
//                ModelState.Clear();
//                TempData["alertsuccess"] = "Data Submitted !";
//                // loadbag();
//                return RedirectToAction("ViewBill", "EBill", new { id = TempData["detailviewID"] });
//            }

//            return View();
//        }


//        public ActionResult CreateItem(Items item)
//        {
//            if (ModelState.IsValid)
//            {
//                return PartialView("_CreateItem", item);
//            }
//            else
//            {
//                TempData["crebillalert"] = "All Fields are required !";
//                return View();
//            }

//        }

//        [Route("ViewBill")]
//        [Authorize]
//        public ActionResult ViewBill(int id)
//        {
//            Data data = new Data();
//            var details = data.GetoneBillDetail(id);
//            return View(details);
//        }

//        // [Route("{Id}")]
//        [Route("DeleteRow")]
//        public ActionResult DeleteRow(int? Id)
//        {
//            //Data data1 = new Data();
//            //var rowid = data1.Deletedata(Id);
//            //if (rowid)
//            //{
//            //    TempData["alert"] = "Deleted !";
//            //}
//            //else
//            //{
//            //    TempData["alert"] = "Not Deleted !";
//            //}
//try
//{
//    sqlparameter[] parameters1 = new sqlparameter[]
//    {

//                new sqlparameter("@id", id),

//    };


//    var isvalid1 = (int)us.func_executescalar("delete_data", parameters1);
//    if (isvalid1 > 0)
//    {
//        modelstate.clear();
//        tempdata["deletealert"] = "deleted !";

//    }
//    else
//    {

//        tempdata["deletealert"] = "not deleted !";

//    }
//}
//catch (exception ex)
//{
//    tempdata["deletealert"] = ex.message;
//}

//return redirecttoaction("index", "ebill");
//        }

//        public ActionResult Edit()
//        {
//            Response.Cache.SetCacheability(HttpCacheability.NoCache);
//            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
//            Response.Cache.SetNoStore();
//            if (Request.IsAuthenticated && Request.Cookies["Email"] != null)
//            {
//                TempData["logindata"] = Request.Cookies["Email"].Value;
//                var userEmail = Request.Cookies["Email"].Value;
//                Response.Cookies["Email"].Value = userEmail;
//            }

//            return View();
//        }

//        [HttpPost]
//        [Authorize]
//        public ActionResult Edit(Edit edit)
//        {

//            SqlParameter[] parameters1 = new SqlParameter[]
//            {
//               new SqlParameter("@password", edit.password),
//            };

//            var isValid = (int)us.func_ExecuteScalar("ckeckpassw", parameters1);

//            if (isValid > 0)
//            {

//                SqlParameter[] parameters2 = new SqlParameter[]
//                {
//                     new SqlParameter("@password", edit.password),
//                };


//                var id = (int)us.func_ExecuteScalar("ckeckpassw", parameters2);


//                // TempData["password"] = edit.password;
//                ModelState.Clear();
//                TempData["showEdit"] = "Successfully Verified !";


//                return RedirectToAction("editRow", "EBill", new { id = id });
//            }
//            else
//            {

//                TempData["messEdit"] = "Enter correct Password !";
//                return View();
//            }


//            // return View();
//        }

//        public ActionResult editRow(int Id)
//        {
//            Response.Cache.SetCacheability(HttpCacheability.NoCache);
//            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
//            Response.Cache.SetNoStore();
//            if (Request.IsAuthenticated && Request.Cookies["Email"] != null)
//            {
//                TempData["logindata"] = Request.Cookies["Email"].Value;
//                var userEmail = Request.Cookies["Email"].Value;
//                Response.Cookies["Email"].Value = userEmail;
//            }
//            List<editRow> res = new List<editRow>();
//            SqlParameter[] parameters = new SqlParameter[]
//               {
//                    new SqlParameter("@Id",Id),


//               };

//            var res1 = us.fn_DataTable("getOneEbillDetails", parameters).AsEnumerable().Select(s => new editRow
//            {

//                Id = s.Field<int>("BillId"),
//                Cust_Name = s.Field<string>("Cust_Name"),
//                Mobile = s.Field<string>("Mobile"),
//                Address = s.Field<string>("Address")


//            }).ToList();
//            ViewBag.editrow = res1;
//            return View();
//        }

//        [HttpPost]
//        public ActionResult editRow(editRow ed)
//        {


//            return View();
//        }


//    }
//}