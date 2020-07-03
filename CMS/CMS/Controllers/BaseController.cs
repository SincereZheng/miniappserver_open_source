using CommonLibrary;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace CMS.Controllers
{
    public class BaseController : Controller
    {
        private static List<HashObject> Users = new List<HashObject>();
        public BaseController()
        {
            if (Users.Count == 0)
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select * from T_LoginUser";
                    DataTable dt = db.ExecuteSqlWithOneTable<DataTable>(sql);
                    Users = dt.ToHashObjectList();
                }
            }
        }
        public void ClearSession()
        {
            Session.Clear();
        }
        [WebMethod]
        public JsonResult CheckLogin()
        {

            if (Session["CurrentUser"] == null)
                return Json(false, JsonRequestBehavior.DenyGet);
            return Json(true, JsonRequestBehavior.DenyGet);
        }

        public UserInfo GetUserByCode(string code)
        {
            HashObject u = Users.FirstOrDefault(x => x.GetValue<string>("UserCode", "") == code);
            UserInfo result = new UserInfo();
            if (u != null)
            {
                result.Id = u.GetValue<int>("Id");
                result.UserCode = u.GetValue<string>("UserCode");
                result.Name = u.GetValue<string>("Name");
                result.IsManager = u.GetValue<int>("IsManager", 0) == 1;
                result.UType = u.GetValue<int>("UType");
                result.WeChatOpenId = u.GetValue<string>("WeChatOpenId");
            }
            return result;
        }
        #region override方法
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            //斜杠在前 "/miniapp"
            string virtualname = Request.ApplicationPath.Replace("/","");
            if (!string.IsNullOrEmpty(virtualname))
                virtualname = Request.ApplicationPath;

            string url = Request.RawUrl.ToLower().Substring(virtualname.Length);
            ViewData["virtualname"] = virtualname;
            if (url != "/login/index" && url != "/login/dologin" && url != "/base/checklogin")
            {
                if(Session["CurrentUser"] == null)
                {
                    bool isajax = Convert.ToBoolean(Request.Params["ajax"]);
                    if (isajax)
                        Response.Write("l0912");
                    else
                    {
                            Response.Redirect(virtualname + "/Login/Index");
                    }
                    Response.End();
                }
            }
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if(filterContext.Result.GetType() == typeof(JsonResult))
            {
                var result = (JsonResult)filterContext.Result;
                JsonSerializerSettings setting = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                if (result.Data.GetType() == typeof(DataTable))
                {
                    result.Data = JsonConvert.SerializeObject(result.Data, setting);
                }
                else if (result.Data.GetType() == typeof(HashObject))
                {
                    result.Data = JsonConvert.SerializeObject(result.Data, setting);
                }
                else if (result.Data.GetType() == typeof(List<HashObject>))
                {
                    result.Data = JsonConvert.SerializeObject(result.Data, setting);
                }
                //else if (result.Data.GetType() == typeof(jqGridRetListObject))
                //{
                //    result.Data = JsonConvert.SerializeObject(result.Data, setting);
                //}
                filterContext.Result = result;
            }
            //filterContext.
            base.OnActionExecuted(filterContext);
        }
        // GET: Base
        protected override void OnException(ExceptionContext filterContext)
        {
            bool isajax = Convert.ToBoolean(Request.Params["ajax"]);
            if (isajax)
            {
                //前端封装ajax，传递默认参数isInvoke为true
                bool isInvok = Convert.ToBoolean(Request.Params["isInvoke"]);
                filterContext.ExceptionHandled = true;
                base.OnException(filterContext);
                if (isInvok)
                {
                    Response.Write(filterContext.Exception.Message);
                }
                else
                    Response.Write("<script>alert('" + filterContext.Exception.Message + "')</script>");
                Response.StatusCode = 500;
            }
            else
                Response.Write(filterContext.Exception.Message);
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            WriteErrLog(filterContext.RouteData.Values["controller"].ToString() + "/" + filterContext.RouteData.Values["action"].ToString(), Request.Params, filterContext.Exception);
            Response.End();
        }
        #endregion

        #region 记录日志到数据库表
        protected void WriteInfoLog(string func, System.Collections.Specialized.NameValueCollection para, string responsedata)
        {
            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@logtype", "info");
                db.AddParameter("@function", func);
                db.AddParameter("@requestpara", para.ToString());
                db.AddParameter("@responsedata", responsedata);
                db.AddParameter("@errmsg", "");
                db.AddParameter("@stacktrace", "");

                db.ExecuteProcedure<DataTable>("P_AddLog");
            }
        }

        protected void WriteErrLog(string func, System.Collections.Specialized.NameValueCollection para, Exception e)
        {
            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@logtype", "error");
                db.AddParameter("@function", func);
                db.AddParameter("@requestpara", para.ToString());
                db.AddParameter("@responsedata", "");
                db.AddParameter("@errmsg", e.Message);
                db.AddParameter("@stacktrace", e.StackTrace);

                db.ExecuteProcedure<DataTable>("P_AddLog");
            }
        }
        #endregion


        #region 文件操作
        protected List<string> SaveFile(HttpFileCollectionBase files,bool onlysavefirst = true)
        {
            List<string> urls = new List<string>();
            for(int i = 0; i < files.Count; i++)
            {
                if (onlysavefirst && i != 0)
                    break;
                var file = files[i];
                string fileNewName = DateTime.Now.ToString("yyyyMMddHHmmssff") + System.IO.Path.GetExtension(file.FileName);
                //保存文件
                if (!Directory.Exists(Server.MapPath("~/Files")))
                    Directory.CreateDirectory(Server.MapPath("~/Files"));
                string path = Server.MapPath("~/Files/" + fileNewName);
                file.SaveAs(path);
                string url = "../Files/" + fileNewName;
                urls.Add(url);
            }
            return urls;
        }
        protected string RemoveFileHostAndPort(string url)
        {
            string newurl = "";
            string host =Request.UrlReferrer.Scheme + "://" + Request.UrlReferrer.Authority;
            if (!String.IsNullOrEmpty(url))
            {
                if (url.StartsWith(host))
                {
                    newurl = ".." + url.Substring(host.Length);
                    if(Request.ApplicationPath.Length > 1)
                        newurl = newurl.Replace(Request.ApplicationPath, "");
                    return newurl;
                }
            }
            return url;
        }
        protected void DeleteUselessImg()
        {
            using (DbHelper db = new DbHelper())
            {
                List<DataTable> dts = db.ExecuteProcedure<DataTable>("P_GetUsedImageFile");
                List<Img> imgs = JsonConvert.DeserializeObject<List<Img>>(dts[0].ToJson());
                DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/Files"));
                foreach(var file in dir.GetFiles())
                {
                    if(imgs.FirstOrDefault(x=>x.Name == Server.UrlEncode(file.Name)) == null)
                    {
                        //增加一个过期时间 一天
                        if ((DateTime.Now - file.CreationTime).TotalHours < 24)
                            continue;
                        System.IO.File.Delete(file.FullName);
                    }
                }
            }
        }
        #endregion
        protected string ReplaceSpecialChar(string AText)
        {
            if (String.IsNullOrEmpty(AText))
                return string.Empty;
            AText = AText.Replace("&", "&amp;");
            AText = AText.Replace("<", "&lt;");
            AText = AText.Replace("''", "&apos;");
            AText = AText.Replace("\"", "&quot;");
            return AText;
        }
    }
    public class Img
    {
        public string Name { get; set; }
    }
    public class UserInfo
    {
        public int Id { get; set; }
        public string UserCode { get; set; }
        public string Name { get; set; }
        public bool IsManager { get; set; }
        public int UType { get; set; }
        public string WeChatOpenId { get; set; }
    }
}