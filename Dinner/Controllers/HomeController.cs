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

        [Authorize]
        public ActionResult Browse()
        {
            var userId = User.Identity.GetUserId();
            var cCouple = db.Couples.FirstOrDefault(c => c.CurrentUser == userId);
            //Viewing Couples to match with
            var r = new Random();
            var list = db.Couples.Where(c => c.Orientation == cCouple.Orientation
            && c.CurrentUser != userId 
            ).ToList();
            ViewBag.AllCouples = list.OrderBy(x => r.Next());
            return View();
        }
    }
}