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
            var thisCouple = db.Couples.FirstOrDefault(c => c.CurrentUser == userId);
            var r = new Random();
            //Couples that match sexual orientation
            var list1 = db.Couples.Where(c => c.Orientation == thisCouple.Orientation
            && c.CurrentUser != userId 
            ).ToList();
            //Removing liked and disliked couples
            var listDislike = db.Dislikes.Where(c => c.ThisCouple == thisCouple.Id);
            var listLike = db.Like.Where(c => c.ThisCouple == thisCouple.Id);
            var list3 = new List<Couple>();
            foreach (var item in listDislike)
            {
                list3.Add(item.Second);
            }
            foreach (var item in listLike)
            {
                list3.Add(item.Second);
            }

            list1.RemoveAll(i => list3.Contains(i));
            ViewBag.AllCouples = list1.OrderBy(x => r.Next());
            return View();
        }
    }
}