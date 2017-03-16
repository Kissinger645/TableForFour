﻿using System;
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

namespace Dinner.Controllers
{
    public class CoupleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Couple
        public ActionResult Index()
        {
            return View(db.Couples.ToList());
        }

        public ActionResult Like(int id)
        {
            var otherCouple = db.Couples.Where(c => c.Id == id).FirstOrDefault().UserName;
            var us = User.Identity.GetUserName();
            MatchedCouple match = new MatchedCouple
            {
                FirstCouple = us,
                SecondCouple = otherCouple,

            };
            bool magic = db.Match.Where(c => c.FirstCouple == otherCouple && c.SecondCouple == us).Any();
            if (magic == true)
            {
                //Send message for match
                ViewBag.Match = "Booyah!";
            }
            return View();
        }

        

        [Route("c/{UserName}")]
        public ActionResult Info(string UserName)
        {
            var UN = UserName;
            ViewBag.ThisCouple = db.Couples.Where(c => c.UserName == UN).FirstOrDefault();
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
            uploadedFile.SaveAs(fullPath);

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
        public ActionResult AboutUs([Bind(Include = "Id,UserName,Bio,ZipCode,Phone,Age,Orientation,FavoriteFoods,AgePreference,SexualPreference,PricePreference")] Couple couple)
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
        public ActionResult Edit([Bind(Include = "Id,UserName,Bio,ZipCode,Phone,Age,Orientation,FavoriteFoods,AgePreference,SexualPreference,PricePreference")] Couple couple)
        {
            if (ModelState.IsValid)
            {
                db.Entry(couple).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
