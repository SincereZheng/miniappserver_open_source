using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Controllers
{
    public class MainPageController : BaseController
    {
        // GET: MainPage
        public ActionResult Index()
        {
            return View();
        }
    }
}