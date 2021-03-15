using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Team1CentricAplication.DAL;
using Team1CentricAplication.Models;
using PagedList;

namespace Team1CentricAplication.Controllers
{
    public class ProfilesController : Controller
    {
        private Team1Context db = new Team1Context();
        // GET: Profiles
        public ActionResult Index(int? page, string searchString)
        {
            int pgSize = 10;
            int pageNumber = (page ?? 1);
            var Profile = from r in db.Profiles select r;
            // sort the records
            Profile = db.Profiles.OrderBy(r => r.lastName).ThenBy(r => r.firstName); ;
            // check to see if a search was requested and do it
            if (!String.IsNullOrEmpty(searchString))
            {
                Profile = Profile.Where(r => r.lastName.Contains(searchString) || r.firstName.Contains(searchString));
            }
            var profileList = Profile.ToPagedList(pageNumber, pgSize);
            return View(profileList);
        }

        // GET: Profiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profiles profiles = db.Profiles.Find(id);
            if (profiles == null)
            {
                return HttpNotFound();
            }
            return View(profiles);
        }

        // GET: Profiles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Profiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "profilesID,firstName,lastName,phone,email,city,zip")] Profiles profiles)
        {
            if (ModelState.IsValid)
            {
                // Guid profilesID;
                // Guid.TryParse(User.Identity.GetUserId(), out profilesID);
                // profiles.profilesID = profilesID;
                db.Profiles.Add(profiles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(profiles);
        }

        // GET: Profiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profiles profiles = db.Profiles.Find(id);
            if (profiles == null)
            {
                return HttpNotFound();
            }
            return View(profiles);
        }

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "profilesID,firstName,lastName,phone,email,city,zip")] Profiles profiles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profiles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(profiles);
        }

        // GET: Profiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profiles profiles = db.Profiles.Find(id);
            if (profiles == null)
            {
                return HttpNotFound();
            }
            return View(profiles);
        }

        // POST: Profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Profiles profiles = db.Profiles.Find(id);
            db.Profiles.Remove(profiles);
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
