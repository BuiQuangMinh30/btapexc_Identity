using ExcIdentity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExcIdentity.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize(Roles = "Admin")]

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();

        }
        [Authorize(Roles =  "Admin")]
        public ActionResult AddRole()
            {
                ViewBag.Message = "Your contact page.";

                return View();
            }
        }
}