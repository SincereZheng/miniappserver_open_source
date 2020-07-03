using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace CMS.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquerythree").Include(
                        "~/Scripts/jquery-3.4.1.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquerytwo").Include(
                        "~/Js/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/base").Include(
                        "~/Js/base.js"));
            bundles.Add(new ScriptBundle("~/bundles/hplusjs").Include(
                        "~/Js/bootstrap.min.js",
                        "~/Js/plugins/metisMenu/jquery.metisMenu.js",
                        "~/Js/plugins/slimscroll/jquery.slimscroll.min.js",
                        "~/Js/plugins/layer/layer.min.js",
                        "~/Js/hplus.js",
                        "~/Js/contabs.js",
                        "~/Js/plugins/pace/pace.min.js"
                        ));
            //bundles.Add(new StyleBundle("~/bundles/hpluscss").Include(
            //    "~/css/font-awesome.min.css",
            //    "~/css/animate.css",
            //    "~/Js/plugins/layer/skin/layer.css",
            //    "~/css/style.css"
            //    ));
            bundles.Add(new StyleBundle("~/bundles/tablelayoutcss").Include(
                "~/Content/css2/app.v2.css",
                "~/Content/css2/style.css",
                "~/Content/css/font-awesome.min93e3.css",
                "~/Content/css/animate.min.css",
                "~/Content/css2/mystyle.css",
                "~/Scripts/plugins/bootstrap-table/bootstrap-table.css",
                "~/Scripts/plugins/bootstrap-table/bootstrap-editable.css",
                "~/Scripts/plugins/bootstrap-table/typeaheadjs/typeahead.js-bootstrap.css",
                "~/Content/css/plugins/toastr/toastr.min.css"
                ));

        }
    }
}