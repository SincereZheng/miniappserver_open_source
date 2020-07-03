using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Controllers
{
    public class ExceptionController : Controller
    {
        // GET: Exception
        public ActionResult Missing(string type,string id)
        {
            ViewData["ErrType"] = type;
            ViewData["ErrMsg"] = id;
            return View();
        }
    }
}