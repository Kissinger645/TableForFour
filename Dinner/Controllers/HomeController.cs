using Dinner.Models;
using Microsoft.AspNet.Identity;
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
            var userId = User.Identity.GetUserId();
            var cCouple = db.Couples.FirstOrDefault(c => c.CurrentUser == userId);
            ViewBag.AllCouples = db.Couples.Where(c => c.Orientation == cCouple.SexualPreference).ToList();
            
            return View();
        }
    }
}