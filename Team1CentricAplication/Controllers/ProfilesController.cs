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
    [Authorize]
    public class ProfilesController : Controller
    {

        private Team1Context db = new Team1Context();
        // GET: Profiles
        [Authorize]
        public ActionResult Index(int? page, string searchString)
        {
            int pgSize = 10;
            int pageNumber = (page ?? 1);
            var Profile = from r in db.Profiles select r;
            // sort the records
            Profile = db.Profiles.Include(p => p.Values).OrderBy(r => r.lastName).ThenBy(r => r.firstName); ;
            // check to see if a search was requested and do it
            if (!String.IsNullOrEmpty(searchString))
            {
                Profile = Profile.Where(r => r.lastName.Contains(searchString) || r.firstName.Contains(searchString));
            }
            var profileList = Profile.ToPagedList(pageNumber, pgSize);
            return View(profileList);
        }

        // GET: Profiles/Details/5
        public ActionResult ProfilesAndValues()
        {
            var profilesList = db.Profiles.Include(o => o.Values).ToList();
            return View("ProfilesAndValues");
        }
        public ActionResult Details(Guid? id)
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
        [Authorize]
        public ActionResult Create()
        {

            return View();
        }

        // POST: Profiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "profilesID,firstName,lastName,phone,email,city,zip,role")] Profiles profiles)
        {
            if (ModelState.IsValid)
            {
                Guid profilesID;
                Guid.TryParse(User.Identity.GetUserId(), out profilesID);
                profiles.profilesID = profilesID;
                profiles.role = Profiles.roles.employee;
                db.Profiles.Add(profiles);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    ViewBag.error = ex.Message;
                    return View("DuplicateUser");
                }
                return RedirectToAction("Index");
            }

            return View(profiles);
        }

        // GET: Profiles/Edit/5
        public ActionResult Edit(Guid? id)
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
            Guid profilesId;
            Guid.TryParse(User.Identity.GetUserId(), out profilesId);
            Profiles loggedInUser = db.Profiles.Find(profilesId);
            bool isAdmin = loggedInUser.role == Profiles.roles.admin;
            if (isAdmin)
            {
                return View(profiles);
            }
            else
            {
                return View("notAuthorized");
            }

        }

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "profilesID,firstName,lastName,phone,email,city,zip,role")] Profiles profiles)
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
        public ActionResult Delete(Guid? id)
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
            Guid profilesId;
            Guid.TryParse(User.Identity.GetUserId(), out profilesId);
            Profiles loggedInUser = db.Profiles.Find(profilesId);
            bool isAdmin = loggedInUser.role == Profiles.roles.admin;
            if (isAdmin)
            {
                return View(profiles);
            }
            else
            {
                return View("AdminOnly");
            }

        }

        // POST: Profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
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
