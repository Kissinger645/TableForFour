using Dinner.Models;
using Microsoft.AspNet.Identity;
using System;
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
            ViewBag.MsgCount = db.Message.Where(c => c.ToCouple == us).Count();
            return View();
        }

        public ActionResult Check(int id)
        {
            var ourId = User.Identity.GetUserId();
            var us = db.Couples.Where(c => c.CurrentUser == ourId).FirstOrDefault();
            //Separating matches to know which is current user
            bool list1 = db.Match.Where(c => c.First.Id == us.Id && c.Second.Id == id).Any();
            bool list2 = db.Match.Where(c => c.Second.Id == us.Id && c.First.Id == id).Any();

            if (list1 || list2 == true)
            {
                return RedirectToAction("New", "Message", new { id = id });
            }

            return RedirectToAction("Sorry", "Message");
            
        }

        public ActionResult New(int id)
        {
            var otherCouple = db.Couples.Where(c => c.Id == id).FirstOrDefault().UserName;
            ViewBag.UN = otherCouple;
            return View();
        }

        [HttpPost]
        public ActionResult New(string message, string title, int id)
        {
            var user = User.Identity.GetUserName();
            var otherCouple = db.Couples.Where(c => c.Id == id).FirstOrDefault();
            Messages newMess = new Messages
            {
                FromCouple = user,
                ToCouple = otherCouple.UserName,
                Title = title,
                Message = message,
                Created = DateTime.Now
            };

            db.Message.Add(newMess);
            db.SaveChanges();
            return RedirectToAction("Browse", "Home");
        }

        public ActionResult Respond(string id)
        {
            var otherCouple = db.Couples.Where(c => c.UserName == id).FirstOrDefault().UserName;
            ViewBag.toThem = otherCouple;
            return View();
        }

        [HttpPost]
        public ActionResult Respond(string message, string title, string id)
        {
            var user = User.Identity.GetUserName();
            var otherCouple = db.Couples.Where(c => c.UserName == id).FirstOrDefault();
            Messages newMess = new Messages
            {
                FromCouple = user,
                ToCouple = otherCouple.UserName,
                Title = title,
                Message = message,
                Created = DateTime.Now
            };

            db.Message.Add(newMess);
            db.SaveChanges();
            return RedirectToAction("Browse", "Home");
        }



        public ActionResult Sorry()
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
