using CommonLibrary;
using DAL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Controllers
{
    public class LoginController : BaseController
    {
        // GET: Login
        public ActionResult Index()
        {
            HashObject model = SystemDal.GetLoginPageInfo();
            return View(model);
        }

        public JsonResult DoLogin()
        {
            string usercode = Request.Params["UserCode"];
            string PassWord = Request.Params["PassWord"];
            using (DbHelper db = new DbHelper())
            {
                db.AddParameter("@UserCode", usercode);
                db.AddParameter("@PassWord", PassWord);
                db.AddParameter("@ReturnCode", 0, System.Data.ParameterDirection.Output);
                HashObject outpara = new HashObject();
                List<DataTable> tables = db.ExecuteProcedureAndOutputParamter<DataTable>("P_CheckLogin", out outpara);
                int retcode = outpara.GetValue<int>("@ReturnCode");
                if (retcode < 0)
                    return Json(tables[0],JsonRequestBehavior.DenyGet);
                else
                {
                    Session["CurrentUser"] = GetUserByCode(usercode);
                    return Json(new DataTable(), JsonRequestBehavior.DenyGet); 
                }
            }
            //JObject jo = new JObject();
        }
    }
}