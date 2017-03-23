using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dinner.Models;
using System.IO;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Migrations;
using System.Text.RegularExpressions;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Configuration;
using ImageResizer;

namespace Dinner.Controllers
{
    [Authorize]
    public class CoupleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Couple
        public ActionResult Index()
        {
            return View(db.Couples.ToList());
        }

        public ActionResult List()
        {
            string us = User.Identity.GetUserId();
            ViewBag.Liked = db.Like.Where(c => c.ThisCouple == us).ToList();
            return View();
        }

        public ActionResult Matches()
        {
            var id = User.Identity.GetUserId();
            //Separating matches to know which is current user
            ViewBag.Matches1 = db.Match.Where(c => c.FirstCouple == id).ToList();
            ViewBag.Matches2 = db.Match.Where(c => c.SecondCouple == id).ToList();
            return View();
        }

        public ActionResult Dislike(string id)
        {
            string otherCouple = db.Users.Where(c => c.UserName == id).FirstOrDefault().Id;
            string us = User.Identity.GetUserId();

            Dislike dislike = new Dislike
            {
                ThisCouple = us,
                OtherCouple = otherCouple
            };

            db.Dislikes.Add(dislike);
            db.SaveChanges();
            var last = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            return Redirect(last);
        }

        public ActionResult Like(string id)
        {
            string otherCouple = db.Users.Where(c => c.UserName == id).FirstOrDefault().Id;
            string us = User.Identity.GetUserId();

            Likes like = new Likes
            {
                ThisCouple = us,
                OtherCouple = otherCouple
            };

            bool magic = db.Like.Where(c => c.ThisCouple == otherCouple && c.OtherCouple == us).Any();
            if (magic == true)
            {
                MatchedCouple match = new MatchedCouple
                {
                    FirstCouple = us,
                    SecondCouple = otherCouple,
                };
                db.Match.Add(match);
                //Generate api search for top restaurants between couples.ZipCode

                //Send Message with Results
                var ourPhone = db.Couples.FirstOrDefault(c => c.CurrentUser == us).Phone;
                var ourName = db.Couples.FirstOrDefault(c => c.CurrentUser == us).UserName;
                var z1 = db.Couples.FirstOrDefault(c => c.CurrentUser == us).ZipCode;
                var theirPhone = db.Couples.FirstOrDefault(c => c.CurrentUser == otherCouple).Phone;
                var theirName = db.Couples.FirstOrDefault(c => c.CurrentUser == otherCouple).UserName;
                var z2 = db.Couples.FirstOrDefault(c => c.CurrentUser == otherCouple).ZipCode;
                string AccountSid = ConfigurationManager.AppSettings["TwilioSID"];
                string AuthToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
                TwilioClient.Init(AccountSid, AuthToken);

                MessageResource.Create(
                to: new PhoneNumber(ourPhone),
                from: new PhoneNumber("+18642077275"),
                body: $"Congrats! You've made a Table For Four match with {theirName}. " +
                $"You can reach them at {theirPhone}. " +
                "Here are some restaurants located between you. " +
                $"https://www.meetways.com/halfway/'{z1}'/'{z2}'/restaurant/d");

                MessageResource.Create(
                to: new PhoneNumber(theirPhone),
                from: new PhoneNumber("+18642077275"),
                body: $"Congrats! You've made a Table For Four match with {ourName}. " +
                $"You can reach them at {ourPhone}. " +
                "Here are some restaurants located between you. " +
                $"https://www.meetways.com/halfway/'{z1}'/'{z2}'/restaurant/d");
            }

            db.Like.Add(like);
            db.SaveChanges();
            var last = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            return Redirect(last);
        }

        [Route("c/{UserName}")]
        public ActionResult Info(string UserName)
        {
            string otherCouple = db.Users.Where(c => c.UserName == UserName).FirstOrDefault().Id;
            string us = User.Identity.GetUserId();
            bool liked = db.Like.Where(c => c.ThisCouple == us && c.OtherCouple == otherCouple).Any();
            if (liked == true)
            {
                ViewBag.Liked = "(You liked this couple)";
            }
            else
            {
                ViewBag.Liked = "";
            }

            bool magic = db.Match.Where(c => c.FirstCouple == otherCouple && c.SecondCouple == us ||
                 c.FirstCouple == us && c.SecondCouple == otherCouple).Any();
            if (magic == true)
            {
                ViewBag.Match = "We matched! Let's do dinner!";
            }
            else
            {
                ViewBag.Match = "";
            }

            ViewBag.ThisCouple = db.Couples.Where(c => c.UserName == UserName).FirstOrDefault();
            //var last = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            //return Redirect(last);
            return View();
        }

        public ActionResult ProfilePic()
        {
            return View();
        }

        public ActionResult Upload()
        {
            var uploadViewModel = new ImageUploadViewModel();
            return View(uploadViewModel);
        }

        [HttpPost]
        public ActionResult Upload(ImageUploadViewModel formData)
        {
            var theseGuys = User.Identity.GetUserName();
            var thisCouple = User.Identity.GetUserId();
            var uploadedFile = Request.Files[0];
            string filename = $"{DateTime.Now.Ticks}{uploadedFile.FileName}";
            var serverPath = Server.MapPath(@"~\Uploads");
            var fullPath = Path.Combine(serverPath, filename);
            //uploadedFile.SaveAs(fullPath);
            //resizing to 640x640
            ResizeSettings resizeSetting = new ResizeSettings
            {
                Width = 640,
                Height = 640,
                Format = "png"
            };
            ImageBuilder.Current.Build(uploadedFile, fullPath, resizeSetting);

            var uploadModel = new ImageUpload
            {
                Caption = theseGuys,
                File = filename
            };
            var currentUser = db.Couples.Where(c => c.CurrentUser == thisCouple).FirstOrDefault();
            currentUser.ProfilePic = uploadModel.Id;
            db.ImageUploads.Add(uploadModel);
            db.SaveChanges();
            return RedirectToAction("Browse", "Home");
        }

        // GET: Couple/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Couple couple = db.Couples.Find(id);
            if (couple == null)
            {
                return HttpNotFound();
            }
            return View(couple);
        }

        // GET: Couple/AboutUs
        public ActionResult AboutUs()
        {
            return View();
        }

        // POST: Couple/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AboutUs([Bind(Include = "Id,UserName,CurrentUser,Bio,ZipCode,Phone,Age,Orientation,FavoriteFoods,AgePreference,SexualPreference,PricePreference")] Couple couple)
        {
            var user = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                couple.CurrentUser = user;
                couple.ProfilePic = 6;
                couple.UserName = User.Identity.GetUserName();
                db.Couples.Add(couple);
                db.SaveChanges();
                return RedirectToAction("ProfilePic", "Couple");
            }

            return View(couple);
        }

        // GET: Couple/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Couple couple = db.Couples.Find(id);
            if (couple == null)
            {
                return HttpNotFound();
            }
            return View(couple);
        }

        // POST: Couple/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,CurrentUser,ProfilePic,Bio,ZipCode,Phone,Age,Orientation,FavoriteFoods,AgePreference,SexualPreference,PricePreference")] Couple couple)
        {
            if (ModelState.IsValid)
            {
                db.Entry(couple).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Browse", "Home");
            }
            return View(couple);
        }

        // GET: Couple/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Couple couple = db.Couples.Find(id);
            if (couple == null)
            {
                return HttpNotFound();
            }
            return View(couple);
        }

        // POST: Couple/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Couple couple = db.Couples.Find(id);
            db.Couples.Remove(couple);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
