using CommonLibrary;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class WxOrderDal
    {
        public static TPhoneRetParam GetUserOrderList(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                db.AddParameter("@dataType", APhonePubParam.Data.GetValue<string>("dataType", ""));
                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_GetUserOrderList");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }
        
        public static TPhoneRetParam OrderCart(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                db.AddParameter("@ucid", APhonePubParam.Data.GetValue<int>("ucid", 0));
                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_OrderCart");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }
        public static TPhoneRetParam OrderBuyNow(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                db.AddParameter("@gid", APhonePubParam.Data.GetValue<int>("goods_id", 0));
                db.AddParameter("@number", APhonePubParam.Data.GetValue<int>("goods_num", 0));
                db.AddParameter("@skuid", APhonePubParam.Data.GetValue<int>("goods_sku_id", 0));
                db.AddParameter("@ucid", APhonePubParam.Data.GetValue<int>("ucid", 0));
                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_OrderBuyNow");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }

        public static TPhoneRetParam SubmitOrderCart(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                db.AddParameter("@retcode", 0, ParameterDirection.Output);
                db.AddParameter("@msg", "", ParameterDirection.Output);
                db.AddParameter("@ucid", APhonePubParam.Data.GetValue<int>("ucid", 0));
                db.ExecuteProcedure<DataTable>("P_SubmitOrderCart");
                result.RetCode = Convert.ToInt32(db.GetParameterValue("@retcode"));
                result.RetMsg = db.GetParameterValue("@msg").ToString();
                //result.RetJson = dt.Rows[0]["Result"].ToString();
                //result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }

        public static TPhoneRetParam GetUserOrderDetail(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@orderid", APhonePubParam.Data.GetValue<int>("order_id", 0));
                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_GetUserOrderDetail");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }
        public static TPhoneRetParam GetGoodsList(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@page", APhonePubParam.Data.GetValue<int>("page", 0));
                db.AddParameter("@sortType", APhonePubParam.Data.GetValue<string>("sortType", ""));
                db.AddParameter("@sortPrice", APhonePubParam.Data.GetValue<int>("sortPrice", 0));
                db.AddParameter("@categoryid", APhonePubParam.Data.GetValue<int>("category_id", 0));
                db.AddParameter("@search", APhonePubParam.Data.GetValue<string>("search", ""));
                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_GetGoodsList");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }
        
        public static TPhoneRetParam SubmitOrderBuyNow(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                db.AddParameter("@gid", APhonePubParam.Data.GetValue<int>("goods_id", 0));
                db.AddParameter("@number", APhonePubParam.Data.GetValue<int>("goods_num", 0));
                db.AddParameter("@skuid", APhonePubParam.Data.GetValue<int>("goods_sku_id",0));
                db.AddParameter("@ucid", APhonePubParam.Data.GetValue<int>("ucid", 0));
                db.AddParameter("@retcode", 0, ParameterDirection.Output);
                db.AddParameter("@msg", "", ParameterDirection.Output);
                db.ExecuteProcedure<DataTable>("P_SubmitOrderBuyNow");
                result.RetCode = Convert.ToInt32(db.GetParameterValue("@retcode"));
                result.RetMsg = db.GetParameterValue("@msg").ToString();
                //result.RetJson = dt.Rows[0]["Result"].ToString();
                //result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }
        
        public static TPhoneRetParam CancelOrder(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                db.AddParameter("@orderid", APhonePubParam.Data.GetValue<int>("order_id", 0));
                db.ExecuteProcedure<DataTable>("P_CancelOrder");
                return result;
            }
        }
        public static TPhoneRetParam UserOrderReceipt(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                db.AddParameter("@orderid", APhonePubParam.Data.GetValue<int>("order_id", 0));
                db.ExecuteProcedure<DataTable>("P_UserOrderReceipt");
                return result;
            }
        }
        public static TPhoneRetParam PayOrder(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            result.RetCode = -10;
            result.RetMsg = "未开放在线支付功能，已将订单状态更改为待发货";

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@orderid", APhonePubParam.Data.GetValue<int>("order_id", 0));
                db.AddParameter("@status", 20);
                db.AddParameter("@expressno", "");
                db.AddParameter("@expresscompany", "");
                db.ExecuteProcedure<DataTable>("P_UpdateOrderStatus");
            }

            return result;
        }
        public static TPhoneRetParam OrderTradeInfo(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();
            List<HashObject> lsho = new List<HashObject>(); 
            string LogisticCode = APhonePubParam.Data.GetValue<string>("expressno", "");
            string ShipperCode = APhonePubParam.Data.GetValue<string>("expresscompany", "");
            using (DbHelper db = new DbHelper())
            {
                DataTable dt = db.ExecuteSqlWithOneTable<DataTable>("select Code,Name From T_KdConfig where Code<>'Other'");
                lsho = dt.ToHashObjectList();
            }
            if(lsho.FirstOrDefault(x=>x.GetValue<string>("Code", "") == ShipperCode) != null)
            {
                KdApiSearch api = new KdApiSearch();
                var wl = api.getOrderTracesByJson(LogisticCode, ShipperCode);
                if (wl != null && wl.Traces != null)
                {
                    wl.Traces = wl.Traces.OrderByDescending(x => x.AcceptTime).ToList();
                }
                result.data = wl.Traces;
                return result;
            }
            else
            {
                string companys = "";
                foreach(var ho in lsho)
                {
                    companys = (String.IsNullOrEmpty(companys) ? "" : companys + "、") + ho.GetValue<string>("Name");
                }
                result.RetCode = -500;
                result.RetMsg = "目前仅支持" + companys + "的快递查询，其他物流公司的快递请复制快递单号通过百度搜索查询";

                return result;
            }
            
        }
        public static TPhoneRetParam BeforeOrderCartCheck(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_BeforeOrderCartCheck");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }
        public static TPhoneRetParam GetUserOrderGoodsComment(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                db.AddParameter("@orderdetailid", APhonePubParam.Data.GetValue<string>("orderdetailid", ""));
                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_GetUserOrderGoodsComment");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }

        public  static TPhoneRetParam UploadFile(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();
            List<string> urls = new List<string>();
            foreach (var key in APhonePubParam.Data.Keys)
            {
                if (APhonePubParam.Data.GetValue<object>(key).GetType() == typeof(System.Net.Http.StreamContent))
                {
                    var file = (System.Net.Http.StreamContent)APhonePubParam.Data.GetValue<object>(key);
                    string fileNewName = DateTime.Now.ToString("yyyyMMddHHmmssff") + ".jpg";
                    //保存文件
                    if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Files")))
                        Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Files"));
                    string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + fileNewName);

                    SaveImgToLocal(path, APhonePubParam.PostStream);

                    string url = "../Files/" + fileNewName;
                    urls.Add(url);

                    
                }
            }
            result.RetJson = Newtonsoft.Json.JsonConvert.SerializeObject(urls);
            return result;
        }
        public static string SaveImgToLocal(string path, Stream fileStr)
        {

            byte[] buffur = new byte[fileStr.Length];
            fileStr.Read(buffur, 0, buffur.Length);
            File.Delete(path); //先删除文件 在进行下载
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                fs.Write(buffur, 0, buffur.Length);
            }
            fileStr.Close();
            return path;
        }
        public static TPhoneRetParam AddComment(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            //下载临时图片
            //string urlstr = APhonePubParam.Data.GetValue<string>("filePath", "");
            //string[] urls = urlstr.Split(',');
            //List<string> imgurls = new List<string>();
            //try
            //{
            //    foreach (var u in urls)
            //    {
            //        var x = u.Split('.');
            //        WebClient myWebClient = new WebClient();
            //        string fileNewName = DateTime.Now.ToString("yyyyMMddHHmmssff") + "." + x[x.Length - 1];
            //        if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Files")))
            //            Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Files"));
            //        string path = System.Web.HttpContext.Current.Server.MapPath("~/Files/" + fileNewName);
            //        myWebClient.DownloadFile(u, path);
            //        imgurls.Add("../Files/" + fileNewName);
            //    }
            //}
            //catch (Exception e)
            //{
            //}
            string urlstr = APhonePubParam.Data.GetValue<string>("filePath", "");
            string[] urls = urlstr.Split(',');

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@orderdetailid", APhonePubParam.Data.GetValue<string>("orderdetailid", ""));
                db.AddParameter("@imgurls", PakageUrl(urls.ToList()));

                db.ExecuteProcedure<DataTable>("P_AddCommentImg");

                db.ClearParatmeter();

                db.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", "")); 
                db.AddParameter("@orderdetailid", APhonePubParam.Data.GetValue<string>("orderdetailid", ""));
                db.AddParameter("@skuid", APhonePubParam.Data.GetValue<string>("skuid", ""));
                db.AddParameter("@star", APhonePubParam.Data.GetValue<string>("star", ""));
                db.AddParameter("@comment", APhonePubParam.Data.GetValue<string>("comment", ""));
                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_AddComment");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }

        private static string PakageUrl(List<string> imgurls)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Root>");
            int index = 0;
            for (int i = 0; i < imgurls.Count; i++)
            {
                var row = imgurls[i];
                if (!string.IsNullOrEmpty(row))
                {
                    index++;
                    sb.Append(string.Format("<Record ImgUrl=\"{0}\"", row));
                    sb.Append(string.Format(" iOrder=\"{0}\"", index));
                    sb.Append(" />");
                }
            }
            sb.Append("</Root>");
            return sb.ToString();
        }

    }
    
}

