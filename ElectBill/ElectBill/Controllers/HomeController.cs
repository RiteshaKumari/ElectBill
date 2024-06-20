using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ElectBill.Models;
using Rotativa;
using QRCoder;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace ElectBill.Controllers
{
    public class HomeController : Controller
    {
        Utility us = new Utility();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ConnectionString);

        private void loadbag()
        {

            List<Customer> Res = new List<Customer>();

            DataSet DS = us.fn_DataSet("ProddropDown");
            var Book = DS.Tables[0];
            var Res2 = Book.AsEnumerable().Select(s => new Customer
            {
                ProductId = s.Field<int>("ProductId"),
                ProductName = s.Field<string>("ProductName"),
                ProductPrice = s.Field<int>("ProductPrice")

            }).ToList();
            //ViewBag.AllProduct = new SelectList(Res2, "ProductId", "ProductName");
            ViewBag.AllProduct = new SelectList(Res2, "ProductName", "ProductName");
        }

        [HttpGet]
        public ActionResult GetPriceForProduct(string productName)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@productName", productName),
                };

                var price = (int)us.func_ExecuteScalar("getPrice", parameters); 

                return Json(price, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["deletealert"] = ex.Message;
                return Json(0, JsonRequestBehavior.AllowGet); 
            }
        }
        public ActionResult Index()
        {
            return View();
        }

        [Route("Login")]
        [HttpGet]
        public ActionResult Login()
        {
            var domain = Request.Url.Host;
            System.Diagnostics.Debug.WriteLine("Current Domain: " + domain);
            Login user = new Login();

            if (Request.Cookies["email"] != null)
            {
              
                List<Login> res = new List<Login>();
                SqlParameter[] parameters = new SqlParameter[]
                   {
                    new SqlParameter("@Email", Request.Cookies["email"].Value),
                   };

                var res1 = us.fn_DataTable("getStaffdata", parameters).AsEnumerable().Select(s => new getStaffdata
                {
                    StaffID = s.Field<int>("StaffId"),
                    StaffName = s.Field<string>("StaffName"),
                    StaffPass = s.Field<string>("StaffPass"),
                    StaffToken = s.Field<string>("StaffToken")
                }).ToList();
                ViewBag.staDetail = res1;
                user.Email = Request.Cookies["email"].Value;
                foreach (var item in ViewBag.staDetail)
                {
                    user.Pass = item.StaffPass;
                }
                return View(user);
               // return RedirectToAction("CreateBill","Home");
            }
          
            return View();
        }


        [Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login log)
        {
          
            if (ModelState.IsValid)
            {
                SqlParameter[] parameters1 = new SqlParameter[]
                    {
                   new SqlParameter("@Email", log.Email),
                   new SqlParameter("@Pass", log.Pass)
                    };
                TempData["logindata"] = log.Email;
                var isValid = (int)us.func_ExecuteScalar("login", parameters1);

                if (isValid > 0)
                {

                    FormsAuthentication.SetAuthCookie(log.Email, true);
                    if (Request.Cookies["Email"] != null)
                    {
                        Response.Cookies["Email"].Value = log.Email;
                    }
                    else
                    {
                        //  HttpCookie emailCookie = new HttpCookie("Email", log.Email);
                        //  emailCookie.Expires = DateTime.Now.AddDays(10);

                        HttpCookie emailCookie = new HttpCookie("Email", log.Email)
                        {
                            Expires = DateTime.Now.AddDays(10),
                            HttpOnly = true,
                            // Secure = false,
                            Domain = Request.Url.Host,
                            Secure = Request.IsSecureConnection,
                           // Domain = "localhost:44307",
                            Path = "/"
                        };
                     //   emailCookie.Secure = false; 
                      //  emailCookie.HttpOnly = true;
                        Response.Cookies.Add(emailCookie);
                    }

                    ModelState.Clear();
                    TempData["mes"] = "done";
                    ViewBag.Messagelog = "Successfully Login !";
                    return RedirectToAction("CreateBill", "Home");

                }
                else
                {
                    ViewBag.Messagelog = "Something went wrong !";
                    return View(log);
                }
            }
            else
            {
                ViewBag.Messagelog = "Incorrect Email or Password !";
                return View(log);
            }

            //  return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            // Clear authentication cookie
            if (Request.Cookies["Email"] != null)
            {
                var emailCookie = new HttpCookie("Email")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(emailCookie);
            }



            // Optionally clear session data if using session
            Session.Clear();
            Session.Abandon();

            // Redirect to the home page
            return RedirectToAction("Index", "Home");
        }
      
        [Route("CreateBill")]
        [Authorize]
        [HttpGet]
        public ActionResult CreateBill()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            if (ModelState.IsValid)
            {
                if (Request.IsAuthenticated && Request.Cookies["Email"] != null)
                {
                    TempData["logindata"] = Request.Cookies["Email"].Value;
                    var userEmail = Request.Cookies["Email"].Value;


                    Response.Cookies["Email"].Value = userEmail;
                }
                else
                {
                    return Redirect("login");
                }
            }
            loadbag();
            return View();
        }

       
        [Route("CreateBill")]
        [Authorize]
        [HttpPost]

        public ActionResult CreateBill(Customer cust)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
          
                if (Request.IsAuthenticated && Request.Cookies["Email"] != null)
                {
                    TempData["logindata"] = Request.Cookies["Email"].Value;
                    var userEmail = Request.Cookies["Email"].Value;
                    Response.Cookies["Email"].Value = userEmail;
                }

            if (ModelState.IsValid)
            {
                if (cust.Items.Count != 0)
                {
                    try
                    {
                        List<Customer> res = new List<Customer>();
                        SqlParameter[] parameter1 = new SqlParameter[]
                        {
                      new SqlParameter("@Cust_Name", cust.cust_Name),
                      new SqlParameter("@Mobile", cust.cust_Mobile)
                        };
                        var bookingprint = us.fn_DataTable("saveCustDetail", parameter1).AsEnumerable().Select(s => new Customer
                        {
                            cust_Id = s.Field<int>("cust_Id"),
                            OID = s.Field<string>("OID")
                        }).FirstOrDefault();
                        ViewBag.book = bookingprint;
                        TempData["detailviewid"] = bookingprint.cust_Id;

                        if (bookingprint != null)
                        {
                            float totalPrice = 0;

                            foreach (var product in cust.Items)
                            {
                                totalPrice += product.Item_Price * product.Quantity;
                                SqlParameter[] productParameters = new SqlParameter[]
                                {
                              new SqlParameter("@customerId",bookingprint.cust_Id),
                              new SqlParameter("@OID", bookingprint.OID),
                              new SqlParameter("@Item_Name", product.Item_Name),
                              new SqlParameter("@Item_Price", product.Item_Price),
                              new SqlParameter("@Quantity", product.Quantity)
                                };
                                us.fn_DataTable("saveItemDetail", productParameters);

                                SqlParameter[] updateParameters = new SqlParameter[]
                                {
                                new SqlParameter("@cust_Id", bookingprint.cust_Id),
                                new SqlParameter("@Totalprice", totalPrice)
                                };

                                us.fn_DataTable("updateCustTotalPrice", updateParameters);
                                TempData["alertsuccess"] = "Data Saved successfully !";
                              
                            }
                            return RedirectToAction("Invoice", "Home", new { id = TempData["detailviewID"] });
                        }
                        else
                        {
                            TempData["message"] = "Customer Detail not saved !";
                        }


                        ModelState.Clear();


                    }
                    catch (Exception ex)
                    {
                        TempData["message"] = ex.Message;

                    }
                }
                else
                {
                    TempData["message"] = "Please Enter atleast one Item !";
                }
            }
            else
            {
                TempData["message"] = "Please Enter valid data !";
            }

           loadbag();
           return View();
           }

        [Route("DetailInRow")]
        [Authorize]
        public ActionResult DetailInRow()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

            if (ModelState.IsValid)
            {
                if (Request.IsAuthenticated && Request.Cookies["Email"] != null)
                {
                    TempData["logindata"] = Request.Cookies["Email"].Value;
                    var userEmail = Request.Cookies["Email"].Value;


                    Response.Cookies["Email"].Value = userEmail;
                }
                else
                {
                    return Redirect("login");
                }

                List<Customer> Res = new List<Customer>();

                DataSet DS = us.fn_DataSet("DetailInRow");
                var Book = DS.Tables[0];

                var Res1 = Book.AsEnumerable().Select(s => new Customer
                {
                    cust_Id = s.Field<int>("cust_Id"),
                    cust_Name = s.Field<string>("cust_Name"),
                    password = s.Field<string>("password"),
                    DateTime = s.Field<DateTime>("DateTime"),
                    OID= s.Field<string>("OID"),
                    status = s.Field<string>("status"),
                    cust_Mobile = s.Field<string>("cust_Mobile"),
                    totalPrice = s.IsNull("Totalprice") ? (float?)null : Convert.ToSingle(s["Totalprice"]),
                //  status = s.IsNull("status") ||s["status"].ToString().Length != 1 ?   'N' : Convert.ToChar(s["status"])   // Handling invalid status
           
                }).ToList();

                ViewBag.AllBooking = Res1;
            }
            return View();
        }

        [Route("DeleteRow")]
        [Authorize]
        public ActionResult DeleteRow(int? Id, bool? confirm)
        {
          
            if (confirm.HasValue && confirm.Value)
            {
                try
                {
                    SqlParameter[] parameters1 = new SqlParameter[]
                    {

                new SqlParameter("@Id", Id),

                    };

                    var isvalid1 = (int)us.func_ExecuteScalar("delete_data", parameters1);
                    if (isvalid1 > 0)
                    {
                        ModelState.Clear();
                        TempData["deletealert"] = "deleted !";

                    }
                    else
                    {

                        TempData["deletealert"] = "not deleted !";

                    }
                }
                catch (Exception ex)
                {
                    TempData["deletealert"] = ex.Message;
                }
            }
            else
            {
                TempData["deletealert"] = "Deletion was not confirmed.";
            }
            return RedirectToAction("DetailInRow", "Home");
           /// return View();
        }

        [Route("Cancel")]
     
        public ActionResult Cancel(int? Id, bool? confirm)
        {

            if (confirm.HasValue && confirm.Value)
            {
                try
                {
                    SqlParameter[] parameters1 = new SqlParameter[]
                    {

                       new SqlParameter("@Id", Id),

                    };

                    var isvalid1 = (int)us.func_ExecuteScalar("cancel_data", parameters1);
                    if (isvalid1 > 0)
                    {
                        ModelState.Clear();
                        TempData["deletealert"] = "Cancelled !";

                    }
                    else
                    {

                        TempData["deletealert"] = "not Cancelled !";

                    }
                }
                catch (Exception ex)
                {
                    TempData["deletealert"] = ex.Message;
                }
            }
            else
            {
                TempData["deletealert"] = "Deletion was not confirmed.";
            }
            return RedirectToAction("DetailInRow", "Home");
        }

        public Customer GetoneBillDetail(int id)
        {

            Customer detail = new Customer();

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("getOneEbillDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    detail.cust_Id = int.Parse(reader["BillId"].ToString());
                    detail.cust_Name = reader["cust_Name"].ToString();
                    detail.cust_Mobile = reader["cust_Mobile"].ToString();
                    detail.Date = reader["Date"].ToString() ;
                    detail.totalPrice = int.Parse(reader["Totalprice"].ToString());
                   
                    ItemDetail item = new ItemDetail();
                    item.Item_Id = int.Parse(reader["ItemId"].ToString());
                    item.Item_Name = reader["Item_Name"].ToString();
                    item.Item_Price = int.Parse(reader["Item_Price"].ToString());
                    item.Quantity = int.Parse(reader["Quantity"].ToString());
                    detail.Items.Add(item);

                    using ( MemoryStream ms = new MemoryStream())
                    {
                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrData = qrGenerator.CreateQrCode($"Customer Name:{reader["cust_Name"]}, Cust_OID:{reader["OID"]}, Cust_Mobile:{reader["cust_Mobile"]}", QRCodeGenerator.ECCLevel.Q);
                        QRCode qRCode = new QRCode(qrData);
                        using (Bitmap bitmap = qRCode.GetGraphic(20)) //-----20 ke jagah id bhi ho sakta
                        {
                            bitmap.Save(ms, ImageFormat.Png);
                            ViewBag.QRCodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
              

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
            return detail;
        }

        [Route("ConvertToPDF")]
        public ActionResult ConvertToPDF(int id)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

            if (Request.IsAuthenticated && Request.Cookies["Email"] != null)
            {
                TempData["logindata"] = Request.Cookies["Email"].Value;
                var userEmail = Request.Cookies["Email"].Value;
                Response.Cookies["Email"].Value = userEmail;
                var printpdf = new ActionAsPdf("InvoiceDownload", new { id = id, email = userEmail });
                return printpdf;
            }
            else
            {
                return Redirect("login");
            }
        }
        [Route("InvoiceDownload")]
        [Authorize]
        public ActionResult InvoiceDownload(int id, string email)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

            if (Request.IsAuthenticated && !string.IsNullOrEmpty(email))
            {
                var userEmail = email;
                Response.Cookies["Email"].Value = userEmail;
            }
            else
            {
                return Redirect("login");
            }

            var details = GetoneBillDetail(id);
            return View(details);
        }


        [Route("Invoice")]
        public ActionResult Invoice(int id)
        {
             
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
                if (Request.IsAuthenticated && Request.Cookies["Email"] != null)
                {
                    TempData["logindata"] = Request.Cookies["Email"].Value;
                    var userEmail = Request.Cookies["Email"].Value;


                    Response.Cookies["Email"].Value = userEmail;
                }
            else
            {
                return Redirect("login");
            }

            var details = GetoneBillDetail(id);
            return View(details);
          
        }

       


        [Route("DeleteItem")]
        public ActionResult DeleteItem(int? Id, bool? confirm)
        {

            if (confirm.HasValue && confirm.Value)
            {
                try
                {
                    SqlParameter[] parameters1 = new SqlParameter[]
                    {

                new SqlParameter("@Id", Id),

                    };

                    var isvalid1 = (int)us.func_ExecuteScalar("delete_item", parameters1);
                    TempData["custID"] = isvalid1;
                    if (isvalid1 > 0)
                    {
                        ModelState.Clear();
                        TempData["deletealert"] = "deleted !";

                    }
                    else
                    {

                        TempData["deletealert"] = "not deleted !";

                    }
                }
                catch (Exception ex)
                {
                    TempData["deletealert"] = ex.Message;
                }
            }
            else
            {
                TempData["deletealert"] = "Deletion was not confirmed.";
            }
           return RedirectToAction("EditBill", "Home", new {id = TempData["custID"] });
           // return View();
        }

        [Route("EditBill")]
        [Authorize]
        public ActionResult EditBill(string OID)
        {
             Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            if (Request.IsAuthenticated && Request.Cookies["email"] != null)
            {
                TempData["logindata"] = Request.Cookies["email"].Value;
                var useremail = Request.Cookies["email"].Value;
                Response.Cookies["email"].Value = useremail;
            }
            else
            {
                return Redirect("login");
            }
            List<editBill> res = new List<editBill>();
            SqlParameter[] parameters = new SqlParameter[]
               {
                    new SqlParameter("@OID",OID),
               };

            var res1 = us.fn_DataTable("getCustDetail", parameters).AsEnumerable().Select(s => new editBill
            {

                BillId = s.Field<int>("cust_Id"),
                OID = s.Field<string>("OID"),
                cust_Name = s.Field<string>("cust_Name"),
                cust_Mobile = s.Field<string>("cust_Mobile")
               


            }).ToList();
            ViewBag.editcustDetail = res1;

        //----------------------------------------------------------

            SqlParameter[] parameter1 = new SqlParameter[]
            {
                        new SqlParameter("@OID", OID),

            };
            DataSet DS = us.fn_DataSet("geteditedCustItems", parameter1);
            var Book = DS.Tables[0];

            var Res1 = Book.AsEnumerable().Select(s => new ItemDetail
            {
                Item_Id = s.Field<int>("Item_Id"),
                Item_Name = s.Field<string>("Item_Name"),
                OID = s.Field<string>("OID"),
                Item_Price = s.Field<int>("Item_Price"),
                Quantity = s.Field<int>("Quantity")

            }).ToList();

            ViewBag.ItemDetail = Res1;


            return View();
        }
        [Authorize]
        [HttpPost]
        [Route("EditBill")]
        public ActionResult EditBill(string OID, Customer cust)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            if (Request.IsAuthenticated && Request.Cookies["email"] != null)
            {
                TempData["logindata"] = Request.Cookies["email"].Value;
                var useremail = Request.Cookies["email"].Value;
                Response.Cookies["email"].Value = useremail;
            }
            try
            {

                //SqlParameter[] parameters = new SqlParameter[]
                //       {
                //        new SqlParameter("@OID", OID),
                //    };
                //var isValid = (int)us.func_ExecuteScalar("deleteWholeRows", parameters);

                float totalPrice = 0;

                foreach (var product in cust.Items)
                {
                    totalPrice += product.Item_Price * product.Quantity;
                    SqlParameter[] productParameters = new SqlParameter[]
                    {
                             
                              new SqlParameter("@OID", OID),
                              new SqlParameter("@Item_Name", product.Item_Name),
                              new SqlParameter("@Item_Price", product.Item_Price),
                              new SqlParameter("@Quantity", product.Quantity)
                    };
                    us.fn_DataTable("saveEditedItemDetail", productParameters);

                    SqlParameter[] updateParameters = new SqlParameter[]
                    {
                                new SqlParameter("@OID", OID),
                                new SqlParameter("@Totalprice", totalPrice)
                     };

                    us.fn_DataTable("updateEditedTotalPrice", updateParameters);
                    TempData["editalert"] = "Data Saved successfully !";
                }
                }
            catch (Exception ex)
            {
                TempData["editalert"] = ex.Message;
            }
          // return RedirectToAction("EditBill", "Home", new { OID = OID });
          return RedirectToAction("DetailInRow","Home");
        }

    }
}