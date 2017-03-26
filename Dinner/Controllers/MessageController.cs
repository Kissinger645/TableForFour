using Dinner.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Dinner.Controllers
{

    public class MessageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public ActionResult List()
        {
            var us = User.Identity.GetUserName();
            ViewBag.Messages = db.Message.Where(c => c.ToCouple == us).ToList();
            return View();
        }

        public ActionResult NewMessage()
        {
            return View();
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Messages message = db.Message.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }

            db.Message.Remove(message);
            db.SaveChanges();
            return RedirectToAction("List");
        }


    }
}
