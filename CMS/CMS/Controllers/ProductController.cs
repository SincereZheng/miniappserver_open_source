using CommonLibrary;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace CMS.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Product
        public ActionResult List()
        {
            return View();
        }
        public ActionResult Content()
        {

            return View();
        }
        [WebMethod]

        public JsonResult GetContent()
        {
            string gid = Request.Params["gid"];
            string content = "";
            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@gid", gid);

                DataTable dt = db.ExecuteSqlWithOneTable<DataTable>("Select id,content from T_Goods where id =@gid");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        content = dt.Rows[0]["content"].ToString();
                    }
                }
                return Json(content, JsonRequestBehavior.AllowGet);
            }
        }
        [ValidateInput(false)]
        [WebMethod]
        public JsonResult EditContent()
        {
            string gid = Request.Params["gid"];
            string content = Request.Params["content"];

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@gid", gid);
                db.AddParameter("@content", content);

                db.ExecuteNonSQL("update T_Goods set content = @content where id = @gid");

                string fileurl = PackageUrl(content);
                db.ClearParatmeter();
                db.AddParameter("@goodsid", gid);
                db.AddParameter("@filexml", fileurl);

                db.ExecuteProcedure<DataTable>("P_SaveContentFileUrl");
                try
                {
                    DeleteUselessImg();
                }
                catch (Exception e)
                {

                }

                return Json("保存成功", JsonRequestBehavior.AllowGet);
            }
        }
        [ValidateInput(false)]
        [WebMethod]
        public JsonResult EditGoodsImage()
        {
            string gid = Request.Params["gid"];
            string content = Request.Params["content"];

            using (DbHelper db = new DbHelper())
            {

                string fileurl = PackageUrl(content);

                db.AddParameter("@goodsid", gid);
                db.AddParameter("@filexml", fileurl);

                db.ExecuteProcedure<DataTable>("P_SaveGoodsImgUrl");
                try
                {
                    DeleteUselessImg();
                }
                catch (Exception e)
                {

                }

                return Json("保存成功", JsonRequestBehavior.AllowGet);
            }
        }
        [WebMethod]
        public JsonResult GetGoodsImage()
        {
            using (DbHelper db = new DbHelper())
            {
                string gid = Request.Params["gid"];
                db.AddParameter("@goodsid", gid);

                List<object> list = db.ExecuteProcedureWithOneTable<List<object>>("P_GetGoodsBannerImgUrl");

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ImageList()
        {

            string sql = "if exists( select 1 from t_goods) select 1 else select 0";
            using (DbHelper db = new DbHelper())
            {
                int count = db.GetValueFromSql(sql);
                if (count == 0)
                    Response.Redirect("~/Exception/Missing/alert/请先添加商品后再操作", true);
                else
                {
                    sql = "select Title,Id From T_Goods order by iorder";
                    DataTable dt = db.ExecuteSqlWithOneTable<DataTable>(sql);
                    ViewData["Goods"] = dt;
                }
            }
            return View();
        }
        private string PackageUrl(string content)
        {
            Regex reg = new Regex(@"(?is)<a[^>]*?href=(['""]?)(?<url>[^'""\s>]+)\1[^>]*>(?<text>(?:(?!</?a\b).)*)</a>");
            MatchCollection mc = reg.Matches(content);
            StringBuilder sb1 = new StringBuilder();
            sb1.Append("<Root>");
            foreach (Match m in mc)
            {
                sb1.Append(string.Format("<Record FileUrl=\"{0}\"", m.Groups["url"].Value));
                sb1.Append(" />");
            }
            Regex regsrc = new Regex(@"(?i)<img[^>]*?\ssrc\s*=\s*(['""]?)(?<src>[^'""\s>]+)\1[^>]*>");
            MatchCollection mcsrc = regsrc.Matches(content);
            foreach (Match m in mcsrc)
            {
                sb1.Append(string.Format("<Record FileUrl=\"{0}\"", m.Groups["src"].Value));
                sb1.Append(" />");
            }
            Regex regvideo = new Regex(@"(?i)<video[^>]*?\ssrc\s*=\s*(['""]?)(?<src>[^'""\s>]+)\1[^>]*>");
            MatchCollection mcvideo = regvideo.Matches(content);
            foreach (Match m in mcvideo)
            {
                sb1.Append(string.Format("<Record FileUrl=\"{0}\"", m.Groups["src"].Value));
                sb1.Append(" />");
            }
            sb1.Append("</Root>");
            return sb1.ToString();
        }
        [WebMethod]
        public JsonResult GetProductList()
        {
            int page = Convert.ToInt32(Request.Params["page"]);
            int rows = Convert.ToInt32(Request.Params["rows"]);//
            string orderby = Request.Params["sidx"];//column
            string ascdesc = Request.Params["sord"];//desc
            string searchField = Request.Params["searchField"];
            string searchString = Request.Params["searchString"];
            string searchOper = Request.Params["searchOper"];
            jqGridRetListObject ret = new jqGridRetListObject();
            using (DbHelper db = new DbHelper())
            {

                db.AddParameter("@orderby", orderby);
                db.AddParameter("@ascdesc", ascdesc);
                db.AddParameter("@rows", rows);
                db.AddParameter("@page", page);

                db.AddParameter("@searchField", searchField);
                db.AddParameter("@searchString", searchString);
                db.AddParameter("@searchOper", searchOper);

                db.AddParameter("@records", 0, ParameterDirection.Output);
                db.AddParameter("@total", 0, ParameterDirection.Output);
                List<object> table = db.ExecuteProcedureWithOneTable<List<object>>("P_GetProductList");
                ret.records = Convert.ToInt32(db.GetParameterValue("@records"));
                ret.rows = table;
                ret.total = Convert.ToInt32(db.GetParameterValue("@total"));
                ret.page = page;
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
        }
        [WebMethod]
        public JsonResult GetGoodsImageSrc()
        {
            string id = Request.Params["gid"];
            using (DbHelper db = new DbHelper())
            {
                string sql = "select * from T_GoodsBannerImage where goodsid=@id order by iorder";
                db.AddParameter("@id", id);

                List<DataTable> ls = db.ExecuteSqlTables<DataTable>(sql);
                if (ls != null && ls.Count >0)
                {
                    return Json(ls[0].ToJsonLower(), JsonRequestBehavior.DenyGet);
                }
                return Json(new List<string>(), JsonRequestBehavior.DenyGet);
            }
        }
        public ActionResult SkuList()
        {
            string sql = "if exists( select 1 from t_goods) select 1 else select 0";
            using (DbHelper db = new DbHelper())
            {
                int count = db.GetValueFromSql(sql);
                if (count == 0)
                    Response.Redirect("~/Exception/Missing/alert/请先添加商品后再操作", true);
            }
            return View();
        }
        public ActionResult GoodsList()
        {
            return View();
        }

        [WebMethod]
        public JsonResult GoodsListTable()
        {
            using (DbHelper db = new DbHelper())
            {
                int total = 0;
                db.AddParameter("@pageSize", Request["pageSize"]);
                db.AddParameter("@pageIndex", Request["pageIndex"]);
                db.AddParameter("@sort", Request["sort"]);
                db.AddParameter("@order", Request["order"]);
                db.AddParameter("@filter", Request["filter"]);
                db.AddParameter("@isdisplay", Request["isdisplay"]);

                db.AddParameter("@total", total, ParameterDirection.Output);

                List<List<object>> rows = db.ExecuteProcedure<List<object>>("P_GetProductList");
                total = Convert.ToInt32(db.GetParameterValue("@total"));

                var res = new { total = total, rows = rows.Count > 0 ? rows[0].Count > 0? rows[0]:new List<object>() : new List<object>() };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }
        [WebMethod]
        public int GetGoodsOrderIndex()
        {
            using (DbHelper db = new DbHelper())
            {
                string sql = "declare @maxindex int,@count int if exists(select 1 from t_goods) begin select @maxindex = max(iOrder) from t_goods select @count = count(*) from t_goods if @maxindex > @count select @maxindex else select @count end else select 1";
                return db.GetValueFromSql(sql);
            }
        }
        [WebMethod]
        public JsonResult GetProductSkuList()
        {
            using (DbHelper db = new DbHelper())
            {
                int total = 0;
                db.AddParameter("@pageSize", Request["pageSize"]);
                db.AddParameter("@pageIndex", Request["pageIndex"]);
                db.AddParameter("@sort", Request["sort"]);
                db.AddParameter("@order", Request["order"]);
                db.AddParameter("@filter", Request["filter"]);
                db.AddParameter("@isdisplay", Request["isdisplay"]);
                db.AddParameter("@goodsid", Request["goodsid"]);

                db.AddParameter("@total", total, ParameterDirection.Output);

                List<List<object>> rows = db.ExecuteProcedure<List<object>>("P_GetProductSkuList");
                total = Convert.ToInt32(db.GetParameterValue("@total"));

                var res = new { total = total, rows = rows.Count > 0 ? rows[0].Count > 0 ? rows[0] : new List<object>() : new List<object>() };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Edit()
        {
            string Id = Request.Params["Id"];
            string Title = Request.Params["Title"];
            string ProPerty = Request.Params["ProPerty"];
            string DefaultImgUrl = Request.Params["DefaultImgUrl"];

            string iOrder = Request.Params["iOrder"];
            string bDisplay = Request.Params["bDisplay"];
            string Images = Request.Params["Images"];
            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@Id", Id);
                db.AddParameter("@Title", Title);
                db.AddParameter("@ProPerty", ProPerty);
                db.AddParameter("@iorder", iOrder);
                db.AddParameter("@bdisplay", bDisplay);
                db.AddParameter("@outid", "",ParameterDirection.Output);
                db.ExecuteProcedure<DataTable>("P_OpereateProductList");

                string fileurl = PackgeGoodsImage(Images);
                object newid = db.GetParameterValue("@outid");
                db.ClearParatmeter();
                db.AddParameter("@goodsid", newid);
                db.AddParameter("@filexml", fileurl);

                db.ExecuteProcedure<DataTable>("P_SaveGoodsImgUrl");
                try
                {
                    DeleteUselessImg();
                }
                catch (Exception e)
                {

                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        private string PackgeGoodsImage(string images)
        {
            StringBuilder sb1 = new StringBuilder();
            List<string> src = images.Split(',').ToList();
            sb1.Append("<Root>");
            int i = 1;
            foreach (var m in src)
            {
                sb1.Append(string.Format("<Record FileUrl=\"{0}\"", m));
                sb1.Append(string.Format(" iOrder=\"{0}\"", i));
                sb1.Append(" />");
                i++;
            }
            
            sb1.Append("</Root>");
            return sb1.ToString();
        }
        [WebMethod]
        public int UpDownGoods()
        {
            string Id = Request.Params["Id"];
            string display = Request.Params["display"];
            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@id", Id);
                db.AddParameter("@display", display);
                string sql = "update t_goods set bDisplay = @display,dDisplayTime = GETDATE() where id=@id";
                return db.ExecuteIntSQL(sql);
            }
        }
        [WebMethod]
        public int UpDownGoodsSku()
        {
            string Id = Request.Params["Id"];
            string display = Request.Params["display"];
            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@id", Id);
                db.AddParameter("@display", display);
                string sql = "update t_goodsdetail set bDisplay = @display,dDisplayTime = GETDATE() where id=@id";
                return db.ExecuteIntSQL(sql);
            }
        }
        

       [WebMethod]
        public JsonResult DeleteGoods()
        {
            string id = Request.Params["id"];
            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@id", id);
                db.AddParameter("@msg", "", ParameterDirection.Output);
                db.ExecuteProcedure<DataTable>("P_DeleteGoods");
                string msg = db.GetParameterValue("@msg").ToString();
                return Json(msg, JsonRequestBehavior.DenyGet);
            }
        }

        [WebMethod]
        public JsonResult DeleteGoodsDetail()
        {
            string id = Request.Params["id"];
            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@id", id);
                db.AddParameter("@msg", "", ParameterDirection.Output);
                db.ExecuteProcedure<DataTable>("P_DeleteGoodsDetail");
                string msg = db.GetParameterValue("@msg").ToString();
                return Json(msg, JsonRequestBehavior.DenyGet);
            }
        }
        public JsonResult EditSku()
        {
            
            string Id = Request.Params["Id"];
            string Name = Request.Params["Name"];
            string costprice = Request.Params["costprice"];
            string Price = Request.Params["Price"];
            string KuCun = Request.Params["KuCun"];
            string ImgUrl = RemoveFileHostAndPort(Request.Params["ImgDom"]);

            string goodsid = Request.Params["Title"];
            string iOrder = Request.Params["iOrder"];
            string bDisplay = Request.Params["bDisplay"];
            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@Id", Id);
                db.AddParameter("@Name", Name);
                db.AddParameter("@costprice", costprice);
                db.AddParameter("@goodsid", @goodsid);
                db.AddParameter("@price", Price);
                db.AddParameter("@kucun", KuCun);
                db.AddParameter("@imgurl", ImgUrl);
                db.AddParameter("@iorder", iOrder);
                db.AddParameter("@bdisplay", bDisplay);


                db.ExecuteProcedure<DataTable>("P_OpereateProductSkuList");
            }
            DeleteUselessImg();
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Category()
        {
            return View();
        }
        public JsonResult CategoryTable()
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

                List<List<object>> rows = db.ExecuteProcedure<List<object>>("P_GetCategoryListTable");
                total = Convert.ToInt32(db.GetParameterValue("@total"));

                var res = new { total = total, rows = rows.Count > 0 ? rows[0] : new List<object>() };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SaveCategory()
        {
            using (DbHelper db = new DbHelper())
            {
                string msg = "";
                db.AddParameter("@Id", Request["Id"]==null?"0": Request["Id"]);
                db.AddParameter("@ParId", Request["ParId"] == null ? "0" : Request["ParId"]);
                db.AddParameter("@ImgUrl", RemoveFileHostAndPort(Request["ImgDom"]));
                db.AddParameter("@Name", Request["Name"]);
                db.AddParameter("@GoodsIds", Request["GoodsIds"]);
                db.AddParameter("@OrderIndex", Request["OrderIndex"]);

                db.AddParameter("@msg", msg, ParameterDirection.Output);

                List<DataTable> rows = db.ExecuteProcedure<DataTable>("P_SaveCategory");
                msg = Convert.ToString(db.GetParameterValue("@msg"));
                DeleteUselessImg();
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

       [WebMethod]
        public JsonResult GetNextOrderIndex()
        {
            string parid = Request.Params["parid"];
            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@parid", parid);
                return Json(db.ExecuteProcedureReturnValue("P_GetCategoryNextOrderIndex"),JsonRequestBehavior.DenyGet);
            }
        }
        [WebMethod]
        public JsonResult GetNextSkuOrder()
        {
            string goodsid = Request.Params["goodsid"];
            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@goodsid", string.IsNullOrEmpty(goodsid) ? "0" : goodsid);
                return Json(db.ExecuteProcedureReturnValue("P_GetNextSkuOrderIndex"), JsonRequestBehavior.DenyGet);
            }
        }
        [WebMethod]
        public int DeleteCategory()
        {
            string id = Request.Params["id"];
            using (DbHelper db = new DbHelper())
            {
                string sql = "delete from t_category where id=@id";
                db.AddParameter("@id", id);
                return db.ExecuteIntSQL(sql);
            }
        }

        public ActionResult KuCunDetail()
        {
            return View();
        }
        [WebMethod]
        public JsonResult KuCunDetailTable()
        {
            using (DbHelper db = new DbHelper())
            {
                int total = 0;
                db.AddParameter("@pageSize", Request["pageSize"]);
                db.AddParameter("@pageIndex", Request["pageIndex"]);
                db.AddParameter("@sort", Request["sort"]);
                db.AddParameter("@order", Request["order"]);
                db.AddParameter("@filter", Request["filter"]);
                db.AddParameter("@DateStart", Request["DateStart"]);
                db.AddParameter("@DeteEnd", Request["DeteEnd"]);
                db.AddParameter("@GoodsId", Request["GoodsId"]);

                db.AddParameter("@totalcount", total, ParameterDirection.Output);

                List<List<object>> rows = db.ExecuteProcedure<List<object>>("P_KuCunDetailReport");
                total = Convert.ToInt32(db.GetParameterValue("@totalcount"));

                var res = new { total = total, rows = rows.Count > 0 ? rows[0] : new List<object>() };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }
    }
}