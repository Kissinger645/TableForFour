using Dinner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dinner.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Browse()
        {
            ViewBag.AllCouples = db.Couples.ToList();
            ViewBag.Pics = db.ImageUploads.ToList();
            return View();
        }
    }
}