using CMS.Common;
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
    public class OrderController : BaseController
    {
        // GET: Order
        public ActionResult OrderList()
        {
            return View();
        }
        [WebMethod]
        public JsonResult OrderListTable()
        {
            using (DbHelper db = new DbHelper())
            {
                int total = 0;
                db.AddParameter("@pageSize", Request["pageSize"]);
                db.AddParameter("@pageIndex", Request["pageIndex"]);
                db.AddParameter("@sort", Request["sort"]);
                db.AddParameter("@order", Request["order"]);
                db.AddParameter("@filter", Request["filter"]);
                db.AddParameter("@orderstatus", Request["orderstatus"]);

                db.AddParameter("@total", total, ParameterDirection.Output);

                List<List<object>> rows = db.ExecuteProcedure<List<object>>("P_GetOrderList");
                total = Convert.ToInt32(db.GetParameterValue("@total"));

                var res = new { total = total, rows = rows.Count > 0 ? rows[0].Count > 0 ? rows[0] : new List<object>() : new List<object>() };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult OrderDetail(string type,int id)
        {
            ViewData["OrderId"] = id;
            using (DbHelper db = new DbHelper())
            {
                string sql = "select Code,Name From T_KdConfig";
                DataTable dt = db.ExecuteSqlWithOneTable<DataTable>(sql);
                ViewData["KdInfo"] = dt;
            }
            return View();
        }
        [WebMethod]
        public JsonResult OrderDetailListTable()
        {
            using (DbHelper db = new DbHelper())
            {
                int total = 0;
                db.AddParameter("@pageSize", Request["pageSize"]);
                db.AddParameter("@pageIndex", Request["pageIndex"]);
                db.AddParameter("@sort", Request["sort"]);
                db.AddParameter("@order", Request["order"]);
                db.AddParameter("@filter", Request["filter"]);
                db.AddParameter("@orderid", Request["OrderId"]);

                db.AddParameter("@total", total, ParameterDirection.Output);

                List<List<object>> rows = db.ExecuteProcedure<List<object>>("P_GetOrderDetailList");
                total = Convert.ToInt32(db.GetParameterValue("@total"));

                var res = new { total = total, rows = rows.Count > 0 ? rows[0].Count > 0 ? rows[0] : new List<object>() : new List<object>() };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }
        [WebMethod]
        public JsonResult OrderFaHuo()
        {
            using (DbHelper db = new DbHelper())
            {
                string msg = "";
                db.AddParameter("@orderdetailids", Request["OrderDetailIds"]);
                db.AddParameter("@expressno", Request["ExpressNo"]);
                db.AddParameter("@expresscompany", Request["ExpressCompany"]);

                db.AddParameter("@msg", msg, ParameterDirection.Output);

                db.ExecuteProcedure<DataTable>("P_OrderDetailFahuo");
                msg = db.GetParameterValue("@msg") == null ? "" : db.GetParameterValue("@msg").ToString();
                
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        [WebMethod]
        public void GetKdInfo()
        {
            string LogisticCode = Request.Params["ExpressNo"];
            string ShipperCode = Request.Params["ExpressCompany"];

            
        }
    }
}