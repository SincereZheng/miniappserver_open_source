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
    public class WxAppDal
    {
        public static TPhoneRetParam GetWxApp(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {

                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_GetWxApp");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }
        public static TPhoneRetParam GetIndexPageData(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {

                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_GetIndexPageData");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }
        
        public static TPhoneRetParam WxappHelp(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {

                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_WxappHelp");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }
        

        public static TPhoneRetParam GetCategoryLists(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {

                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_GetCategoryLists");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }

        public static TPhoneRetParam GetUserOrderCount(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_GetUserOrderCount");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }
        public static TPhoneRetParam SetCartListChecked(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                db.AddParameter("@cartid", APhonePubParam.Data.GetValue<string>("cartid", ""));
                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_SetCartListChecked");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }
        public static TPhoneRetParam WxUserIsAdmin(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_WxUserIsAdmin");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }

        
        public static TPhoneRetParam UserLogin(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            string openid = GetUserOpenId(APhonePubParam.Data.GetValue<string>("code", ""));
            if (String.IsNullOrEmpty(openid))
                return result;
            using (DbHelper db = new DbHelper())
            {
                var info = JsonConvert.DeserializeObject<WxReturnUserInfo>(APhonePubParam.Data.GetValue<string>("user_info", ""));
                if (info == null)
                    return result;
                db.AddParameter("@openid", openid);
                db.AddParameter("@nickName", info.nickName);
                db.AddParameter("@gender", info.gender);
                db.AddParameter("@language", info.language);
                db.AddParameter("@city", info.city);
                db.AddParameter("@province", info.province);
                db.AddParameter("@country", info.country);
                db.AddParameter("@avatarUrl", info.avatarUrl);
                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_UserLogin");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }

        public static TPhoneRetParam GetCartLists(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_GetCartLists");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }

        public static TPhoneRetParam CartAdd(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                db.AddParameter("@goodsid", APhonePubParam.Data.GetValue<int>("goods_id", 0));
                db.AddParameter("@skuid", APhonePubParam.Data.GetValue<int>("goods_sku_id", 0));
                db.AddParameter("@num", APhonePubParam.Data.GetValue<int>("goods_num", 0));
                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_CartAdd");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.RetMsg = dt.Rows[0]["RetMsg"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }
        
        public static TPhoneRetParam GetGoodsDetail(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@goodsid", APhonePubParam.Data.GetValue<string>("goods_id", ""));
                db.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_GetGoodsDetail");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }
        public static TPhoneRetParam GetGoodsComment(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@goodsid", APhonePubParam.Data.GetValue<string>("goods_id", ""));
                db.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_GetGoodsComment");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }
        public static TPhoneRetParam CartDecAndAdd(TPhonePubParam APhonePubParam)
        {
            TPhoneRetParam result = new TPhoneRetParam();

            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@token", APhonePubParam.Data.GetValue<string>("token", ""));
                db.AddParameter("@type", APhonePubParam.Data.GetValue<string>("oper", ""));
                db.AddParameter("@goodsid", APhonePubParam.Data.GetValue<int>("goods_id", 0));
                db.AddParameter("@skuid", APhonePubParam.Data.GetValue<int>("goods_sku_id", 0));
                db.AddParameter("@number", APhonePubParam.Data.GetValue<int>("goods_num", 0));
                DataTable dt = db.ExecuteProcedureWithOneTable<DataTable>("P_CartDecAndAdd");
                result.RetCode = 0;
                result.RetJson = dt.Rows[0]["Result"].ToString();
                result.data = JsonConvert.DeserializeObject(result.RetJson);
                return result;
            }
        }
        private static string GetUserOpenId(string code)
        {
            string openid = "";
            if (string.IsNullOrEmpty(code))
                return openid;
            using(DbHelper db = new DbHelper())
            {
                DataTable dt = db.ExecuteSqlWithOneTable<DataTable>("Select AppId,AppSecrect From T_MiniAppConfig");
                string appid = dt.Rows[0]["AppId"].ToString();
                string appsecrect = dt.Rows[0]["AppSecrect"].ToString();

                string url = string.Format("https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&grant_type=authorization_code&js_code={2}",
                    appid, appsecrect, code);
                string json = GetContent(url, Encoding.UTF8);
                var info = JsonConvert.DeserializeObject<WxReturnUserOpenIdInfo>(json);
                if (info != null)
                {
                    if(info.errcode == 0)
                    {
                        openid = info.openid;
                    }
                }
                return openid;
            }
        }

        private static string GetContent(string uri, Encoding coding)
        {
            //Get请求中请求参数等直接拼接在url中
            HttpWebRequest request = HttpWebRequest.CreateHttp(uri);
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
            //返回对Internet请求的响应
            WebResponse resp = request.GetResponse();

            //从网络资源中返回数据流
            Stream stream = resp.GetResponseStream();

            StreamReader sr = new StreamReader(stream, coding);

            //将数据流转换文字符串
            string result = sr.ReadToEnd();

            //关闭流数据
            stream.Close();
            sr.Close();

            return result;
        }
    }
    public class WxReturnUserOpenIdInfo
    {
        public string openid { get; set; }
        public string session_key { get; set; }
        public string unionid { get; set; }
        public int errcode { get; set; }
        public string errmsg { get; set; }
    }
    public class WxReturnUserInfo
    {
        public string openid { get; set; }
        public string nickName { get; set; }
        public int gender { get; set; }
        public string language { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string avatarUrl { get; set; }
    }
}
