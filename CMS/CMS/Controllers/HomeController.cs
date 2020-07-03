using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace CMS.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            HashObject model = DAL.SystemDal.GetLoginPageInfo();
            ViewBag.Title = model.GetValue<string>("LoginSystemName", "");
            return View();
        }
        public ActionResult Logout()
        {
            base.ClearSession();
            Response.Redirect("../Login/index");
            return View();
        }
        [WebMethod]
        public JsonResult GetServerConfig()
        {
            using (DbHelper db = new DbHelper())
            {
                DataTable tbs = db.ExecuteProcedureWithOneTable<DataTable>("T_GetServerConfig");
                string json = tbs.ToJsonLower();

                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }
        [WebMethod]
        public JsonResult UpdateAllowNegativeKucun()
        {
            using (DbHelper db = new DbHelper())
            {
                string sql = "update T_WxAppConfig set SubValue = @subvalue where subkey='AllowNegativeKucun'";
                db.AddParameter("@subvalue", Request.Params["subvalue"]);
                int count = db.ExecuteIntSQL(sql);

                return Json(count, JsonRequestBehavior.AllowGet);
            }
        }
        [WebMethod]
        public JsonResult UpdateAllowMultiShareCoupon()
        {
            using (DbHelper db = new DbHelper())
            {
                string sql = "update T_WxAppConfig set SubValue = @subvalue where subkey='AllowMultiShareCoupon' update T_CouponShare set ShareType = @subvalue";
                db.AddParameter("@subvalue", Request.Params["subvalue"]);
                int count = db.ExecuteIntSQL(sql);

                return Json(count, JsonRequestBehavior.AllowGet);
            }
        }
        [WebMethod]
        public JsonResult SaveExeInfo()
        {
            using (DbHelper db = new DbHelper())
            {
                string sql = "update T_CompanyInfo set LoginTitle = @name,LoginSystemName=@name";
                db.AddParameter("@name", Request.Params["Name"]);
                int count = db.ExecuteIntSQL(sql);

                return Json(count, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Board()
        {
            return View();
        }

        [WebMethod]
        public JsonResult GetBoardData(string type,string id)
        {
            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@day", id);
                List<List<object>> ls = new List<List<object>>();
                if (type == "SaleAmount")
                    ls = db.ExecuteProcedure<List<object>>("P_GetBoardData_SaleAmount");
                else if (type == "SaleCount")
                    ls = db.ExecuteProcedure<List<object>>("P_GetBoardData_SaleCount");
                else if (type == "SaleAmountPie")
                {

                    ls = db.ExecuteProcedure<List<object>>("P_GetBoardData_SaleAmountPie");
                }
                else if (type == "SaleCountPie")
                    ls = db.ExecuteProcedure<List<object>>("P_GetBoardData_SaleCountPie");

                return Json(ls, JsonRequestBehavior.AllowGet);
            }
        }
    }
}