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
using System.Web.UI;

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
            var id = User.Identity.GetUserId();
            var us = db.Couples.Where(c =>c.CurrentUser == id).FirstOrDefault();
            ViewBag.Liked = db.Like.Where(c => c.First.Id == us.Id).ToList();
            
            return View();
        }

        public ActionResult Matches()
        {
            var id = User.Identity.GetUserId();
            var us = db.Couples.Where(c => c.CurrentUser == id).FirstOrDefault();
            //Separating matches to know which is current user
            ViewBag.Matches1 = db.Match.Where(c => c.First.Id == us.Id).ToList();
            ViewBag.Matches2 = db.Match.Where(c => c.Second.Id == us.Id).ToList();
            return View();
        }

        public ActionResult Unmatch(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MatchedCouple match = db.Match.Find(id);

            if (match == null)
            {
                return HttpNotFound();
            }
            //Remove from liked list also

            db.Match.Remove(match);
            db.SaveChanges();
            return RedirectToAction("Matches", "Couple");
        }

        public ActionResult Dislike(string id)
        {
            var us = User.Identity.GetUserId();
            var otherCouple = db.Couples.Where(c => c.UserName == id).FirstOrDefault();
            var thisCouple = db.Couples.Where(c => c.CurrentUser == us).FirstOrDefault();

            Dislike dislike = new Dislike
            {
                First = thisCouple,
                Second = otherCouple
            };
            db.Dislikes.Add(dislike);

            Messages dislikeMessage = new Messages
            {
                Created = DateTime.Now,
                ToCouple = thisCouple.UserName,
                Title = $"You disliked {otherCouple.UserName}"
            };
            db.Message.Add(dislikeMessage);
            
            db.SaveChanges();
            var last = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            return Redirect(last);
        }

        public ActionResult Like(string id)
        {
            var us = User.Identity.GetUserId();
            var otherCouple = db.Couples.Where(c => c.UserName == id).FirstOrDefault();
            var thisCouple = db.Couples.Where(c => c.CurrentUser == us).FirstOrDefault();

            Likes like = new Likes
            {
                First = thisCouple,
                Second = otherCouple
            };
            db.Like.Add(like);

            Messages likeMessage = new Messages
            {
                Created = DateTime.Now,
                ToCouple = thisCouple.UserName,
                Title = $"You liked {otherCouple.UserName}"
            };
            db.Message.Add(likeMessage);

            //Couples have liked each other, creates match and sends messages
            bool magic = db.Like.Where(c => c.First.Id == otherCouple.Id && c.Second.Id == thisCouple.Id).Any();
            if (magic == true)
            {
                //Send Message with Results
                var ourPhone = thisCouple.Phone;
                var ourName = thisCouple.UserName;
                var z1 = thisCouple.ZipCode;
                var theirPhone = otherCouple.Phone;
                var theirName = otherCouple.UserName;
                var z2 = otherCouple.ZipCode;

                var mess1 = $"Congrats! You've made a Table For Four match with {theirName}. " +
                $"You can reach them at {theirPhone}. " +
                "The link below will show you some restaurants located between you. " +
                $"https://www.meetways.com/halfway/'{z1}'/'{z2}'/restaurant/d";

                var mess2 = $"Congrats! You've made a Table For Four match with {ourName}. " +
                $"You can reach them at {ourPhone}. " +
                "The link below will show you some restaurants located between you. " +
                $"https://www.meetways.com/halfway/'{z1}'/'{z2}'/restaurant/d";

                string AccountSid = ConfigurationManager.AppSettings["TwilioSID"];
                string AuthToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
                TwilioClient.Init(AccountSid, AuthToken);

                MessageResource.Create(
                to: new PhoneNumber(ourPhone),
                from: new PhoneNumber("+18642077275"),
                body: $"{mess1}");

                MessageResource.Create(
                to: new PhoneNumber(theirPhone),
                from: new PhoneNumber("+18642077275"),
                body: $"{mess2}");

                MatchedCouple match = new MatchedCouple
                {
                    FirstCouple = thisCouple.Id,
                    SecondCouple = otherCouple.Id,
                    Suggestions = $"https://www.meetways.com/halfway/'{z1}'/'{z2}'/restaurant/d",
                };
                db.Match.Add(match);

                Messages message1 = new Messages
                {
                    Created = DateTime.Now,
                    FromCouple = thisCouple.UserName,
                    ToCouple = otherCouple.UserName,
                    Title = $"Congrats! You've matched up with {thisCouple.UserName}",
                    Message = mess1
                };
                db.Message.Add(message1);

                Messages message2 = new Messages
                {
                    Created = DateTime.Now,
                    FromCouple = otherCouple.UserName,
                    ToCouple = thisCouple.UserName,
                    Title = $"Congrats! You've matched up with {otherCouple.UserName}",
                    Message = mess2
                };
                db.Message.Add(message2);
            }

            db.SaveChanges();
            var last = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            return Redirect(last);
        }

        public ActionResult Unlike(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Likes like = db.Like.Find(id);

            if (like == null)
            {
                return HttpNotFound();
            }

            db.Like.Remove(like);
            db.SaveChanges();
            return RedirectToAction("List", "Couple");
        }

        [Route("c/{UserName}")]
        public ActionResult Info(string UserName)
        {
            var id = User.Identity.GetUserId();
            var otherCouple = db.Couples.Where(c => c.UserName == UserName).FirstOrDefault().Id;
            var us = db.Couples.Where(c => c.CurrentUser == id).FirstOrDefault();
            bool liked = db.Like.Where(c => c.First.Id == us.Id && c.OtherCouple == otherCouple).Any();
            if (liked == true)
            {
                ViewBag.Liked = "(You liked this couple)";
            }
            else
            {
                ViewBag.Liked = "";
            }

            bool magic = db.Match.Where(c => c.FirstCouple == otherCouple && c.Second.Id == us.Id ||
                 c.First.Id == us.Id && c.SecondCouple == otherCouple).Any();
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
