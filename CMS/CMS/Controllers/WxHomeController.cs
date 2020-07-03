using CommonLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace CMS.Controllers
{
    public class WxHomeController : BaseController
    {
        // GET: WxHome
        public ActionResult Banner()
        {
            return View();
        }
        public JsonResult BannerTable()
        {
            using (DbHelper db = new DbHelper())
            {
                int total = 0;
                db.AddParameter("@pageSize", Request["pageSize"]);
                db.AddParameter("@pageIndex", Request["pageIndex"]);
                db.AddParameter("@sort", Request["sort"]);
                db.AddParameter("@order", Request["order"]);
                db.AddParameter("@filter", Request["filter"]);

                db.AddParameter("@total", total, ParameterDirection.Output);

                List<List<object>> rows = db.ExecuteProcedure<List<object>>("P_GetBannerListTable");
                total = Convert.ToInt32(db.GetParameterValue("@total"));

                var res = new { total = total, rows = rows.Count > 0 ? rows[0] : new List<object>() };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CouponTable()
        {
            using (DbHelper db = new DbHelper())
            {
                int total = 0;
                db.AddParameter("@pageSize", Request["pageSize"]);
                db.AddParameter("@pageIndex", Request["pageIndex"]);
                db.AddParameter("@sort", Request["sort"]);
                db.AddParameter("@order", Request["order"]);
                db.AddParameter("@filter", Request["filter"]);

                db.AddParameter("@total", total, ParameterDirection.Output);

                List<List<object>> rows = db.ExecuteProcedure<List<object>>("P_GetCouponListTable");
                total = Convert.ToInt32(db.GetParameterValue("@total"));

                var res = new { total = total, rows = rows.Count > 0 ? rows[0] : new List<object>() };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }
        [WebMethod]
        public JsonResult GetGoods(string type)
        {
            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@type", type);
                DataTable tbs = db.ExecuteProcedureWithOneTable<DataTable>("P_GetGoodsSelect");
                string json = tbs.ToJsonLower();
                
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }
        [WebMethod]
        public JsonResult GetGoodsSkus(string type,string id)
        {
            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@type", type);
                db.AddParameter("@goodsid", id);
                DataTable tbs = db.ExecuteProcedureWithOneTable<DataTable>("P_GetGoodsSkusSelect");
                string json = tbs.ToJsonLower();

                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SaveBanner()
        {
            using (DbHelper db = new DbHelper())
            {
                string url = "";

                string msg = "";
                db.AddParameter("@Id", Request["Id"] == null ? "0" : Request["Id"]);
                url =RemoveFileHostAndPort(Request.Params["ImgDom"]);
                string orgurl = Request.Params["OrgImgUrl"];
                
                db.AddParameter("@ImgUrl", url);
                db.AddParameter("@Title", Request["Title"]);

                db.AddParameter("@msg", msg, ParameterDirection.Output);

                List<DataTable> rows = db.ExecuteProcedure<DataTable>("P_SaveBanner");
                msg = Convert.ToString(db.GetParameterValue("@msg"));
                if (url != orgurl && string.IsNullOrEmpty(msg))
                {
                    try
                    {
                        DeleteUselessImg();
                    }
                    catch (Exception e)
                    {

                    }
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }
        [WebMethod]
        public JsonResult SaveCoupon()
        {
            string title = Request.Params["Title"];
            string id = Request.Params["Id"];
            string amount = Request.Params["Amount"];
            string limitamount = Request.Params["LimitAmount"];
            string totalcount = Request.Params["TotalCount"];
            string overdate = Request.Params["OverDate"];
            string ids = Request.Params["IDS"];
            string xml = pakageGoodsSkuIds(ids);
            string msg = "";
            using(DbHelper db = new DbHelper())
            {
                db.AddParameter("@title", title);
                db.AddParameter("@id", id);
                db.AddParameter("@amount", amount);
                db.AddParameter("@limitamount", limitamount);
                db.AddParameter("@totalcount", totalcount);
                db.AddParameter("@overdate", overdate);
                db.AddParameter("@xml", xml);
                db.AddParameter("@msg", "",ParameterDirection.Output);
                db.ExecuteProcedure<DataTable>("P_GenerateCoupon");
                msg = db.GetParameterValue("@msg").ToString();
            }

            return Json(msg,JsonRequestBehavior.DenyGet);
        }
        public ActionResult UserList()
        {
            return View();
        }
        public JsonResult UserTable()
        {
            using (DbHelper db = new DbHelper())
            {
                int total = 0;
                db.AddParameter("@pageSize", Request["pageSize"]);
                db.AddParameter("@pageIndex", Request["pageIndex"]);
                db.AddParameter("@sort", Request["sort"]);
                db.AddParameter("@order", Request["order"]);
                db.AddParameter("@filter", Request["filter"]);

                db.AddParameter("@total", total, ParameterDirection.Output);

                List<List<object>> rows = db.ExecuteProcedure<List<object>>("P_GetWxUserListTable");
                total = Convert.ToInt32(db.GetParameterValue("@total"));

                var res = new { total = total, rows = rows.Count > 0 ? rows[0] : new List<object>() };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }
        [WebMethod]
        public ActionResult ToggleUser(string type,string id)
        {
            using (DbHelper db = new DbHelper())
            {
                string sql = "update T_WxUser set IsAdmin = case when IsAdmin = 1 then 0 else 1 end where user_id=@id";
                db.AddParameter("@id", id);
                db.ExecuteNonSQL(sql);
                return Json("", JsonRequestBehavior.DenyGet);
            }
        }
        [WebMethod]
        public ActionResult StopUsingCoupon(string type,string id)
        {
            using(DbHelper db = new DbHelper())
            {
                string status = type == "using" ? "0" : "1";
                string sql = "update T_Coupon set IsStopUsing ="+ status + " where id=@id";
                db.AddParameter("@id", id);
                db.ExecuteNonSQL(sql);
                return Json("", JsonRequestBehavior.DenyGet);
            }
        }
        public string pakageGoodsSkuIds(string ids)
        {
            List<CouponGoodsSkuObject> list = JsonConvert.DeserializeObject<List<CouponGoodsSkuObject>>(ids);
            StringBuilder sb = new StringBuilder();
            sb.Append("<Root>");
            int index = 0;
            for (int i = 0; i < list.Count; i++)
            {
                var row = list[i];
                if (row != null)
                {
                    index++;
                    sb.Append(string.Format("<Record GoodsId=\"{0}\"", row.GoodsId));
                    sb.Append(string.Format(" SkuId=\"{0}\"", row.SkuId));
                    sb.Append(" />");
                }
            }
            sb.Append("</Root>");
            return sb.ToString();
        }
        public ActionResult Coupon()
        {
            return View();
        }
        public ActionResult GetCouponLimit(string type,int id)
        {
            using(DbHelper db = new DbHelper())
            {
                db.AddParameter("@id", id);
                db.AddParameter("@table", "");
                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_GetCouponLimit");
                string text = dt.Rows[0]["Result"].ToString();
                return Json(text, JsonRequestBehavior.DenyGet);
            }
        }
        [WebMethod]
        public JsonResult SaveBannerImg()
        {

            string url = "";
            if (Request.Files != null)
            {
                if (Request.Files.Count > 0)
                {
                    url = SaveFile(Request.Files)[0];
                }
            }
            return Json(url,JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 使用ajaxImageUpload上传图片。详见https://gitee.com/gouguoyin/ajax-image-upload
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public JsonResult UploadFiles()
        {

            string url = "";
            if (Request.Files != null)
            {
                if (Request.Files.Count > 0)
                {
                    url = SaveFile(Request.Files)[0];
                }
            }
            return Json(new { code=200, src = url}, JsonRequestBehavior.DenyGet);
        }
        [WebMethod]
        public int DeleteBanner()
        {
            string id = Request.Params["id"];
            using (DbHelper db = new DbHelper())
            {
                string sql = "delete from T_WxAppBannerConfig where id=@id";
                db.AddParameter("@id", id);
                return db.ExecuteIntSQL(sql);
            }
        }
        [WebMethod]
        public ActionResult DeleteCoupon(string type,int id)
        {
            using (DbHelper db = new DbHelper())
            {
                string sql = "delete from t_coupon where id=@id delete from T_CouponLimit where couponid=@id delete from T_UserCoupon where couponid=@id";
                db.AddParameter("@id", id);
                db.ExecuteIntSQL(sql);
                return Json(1, JsonRequestBehavior.DenyGet);
            }
        }
        public ActionResult HelpDoc()
        {
            return View();
        }
        [WebMethod]
        public JsonResult HelpListTable()
        {
            using (DbHelper db = new DbHelper())
            {
                int total = 0;

                List<object> rows = db.ExecuteSqlWithOneTable<List<object>>("select * from T_WxHelp order by title");
                total = rows.Count;

                var res = new { total = total, rows = rows.Count > 0 ? rows : new List<object>() };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }
        [WebMethod]
        [ValidateInput(false)]
        public JsonResult SaveHelpDoc()
        {
            using (DbHelper db = new DbHelper())
            {
                string docStr = Request.Params["docstr"];
                List<WxHelpDocObject> ls = JsonConvert.DeserializeObject<List<WxHelpDocObject>>(docStr);
                string xml = PakageData(ls);
                db.AddParameter("@docXml", xml);
                db.AddParameter("@msg", "", ParameterDirection.Output);

                db.ExecuteProcedure<DataTable>("P_SaveWxHelpDoc");

                var res = db.GetParameterValue("@msg");
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }
        public string PakageData(List<WxHelpDocObject> ls)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Root>");
            int index = 0;
            for (int i = 0; i < ls.Count; i++)
            {
                var row = ls[i];
                if (row != null)
                {
                    index++;
                    sb.Append(string.Format("<Record Title=\"{0}\"", ReplaceSpecialChar(row.Title)));
                    sb.Append(string.Format(" Content=\"{0}\"", ReplaceSpecialChar(row.Content)));
                    sb.Append(" />");
                }
            }
            sb.Append("</Root>");
            return sb.ToString();
        }
    }
    public class WxHelpDocObject
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
    public class CouponGoodsSkuObject
    {
        public int GoodsId { get; set; }
        public int SkuId { get; set; }
    }
}