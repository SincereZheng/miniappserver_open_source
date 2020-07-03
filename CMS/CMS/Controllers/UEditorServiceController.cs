using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Controllers
{
    public class UEditorServiceController : BaseController
    {
        // GET: UEditorService
        public ActionResult Process()
        {
            object res;
            string action = Request.Params["action"];
            res = "";
            if (action == "uploadvideo")
            {
                var files = System.Web.HttpContext.Current.Request.Files;
                res = "xx";
            }
            else if (action == "config")
            {
                res = new object() { };
            }
            else if(action == "Image" || action == "Video")
            {
                string url = "";
                if (Request.Files != null)
                {
                    if (Request.Files.Count > 0)
                    {
                        url = SaveFile(Request.Files)[0];
                    }
                }
                res = new
                {
                    src=url,
                    url=url,
                    state= "SUCCESS"
                };
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}