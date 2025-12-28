using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BT.ETS.API.Controllers
{
    public class ErrorMapperController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Error Lookup";

            return View();
        }
    }
}
