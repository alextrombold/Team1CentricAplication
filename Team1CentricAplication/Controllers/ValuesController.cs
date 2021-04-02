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
    public class ValuesController : Controller
    {

        private Team1Context db = new Team1Context();

        // GET: Values
        [Authorize]
        public ActionResult Index()
        {
            var values = db.Values;
            return View(values.ToList());
        }

        // GET: Values/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Values values = db.Values.Find(id);
            if (values == null)
            {
                return HttpNotFound();
            }
            return View(values);
        }

        // GET: Values/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.profilesID = new SelectList(db.Profiles, "profilesID", "fullName");
            return View();
        }

        // POST: Values/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "valuesId,nominatedValues,recognizor,recognitionNote,recognitionDate,profilesID")] Values values)
        {
            if (ModelState.IsValid)
            {
                Guid profilesID;
                Guid.TryParse(User.Identity.GetUserId(), out profilesID);
                values.recognizor = profilesID;
                values.recognizationDate = DateTime.Now;
                db.Values.Add(values);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.profilesID = new SelectList(db.Profiles, "profilesID", "fullName", values.AwardNominee);
            return View(values);
        }

        // GET: Values/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Values values = db.Values.Find(id);
            if (values == null)
            {
                return HttpNotFound();
            }
            ViewBag.profilesID = new SelectList(db.Profiles, "profilesID", "fullName", values.AwardNominee);
            return View(values);
        }

        // POST: Values/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "valuesId,nominatedValues,recognizor,recognitionNote,recognitionDate,profilesID")] Values values)
        {
            if (ModelState.IsValid)
            {
                db.Entry(values).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.profilesID = new SelectList(db.Profiles, "profilesID", "fullName", values.AwardNominee);
            return View(values);
        }

        // GET: Values/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Values values = db.Values.Find(id);
            if (values == null)
            {
                return HttpNotFound();
            }
            return View(values);
        }

        // POST: Values/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Values values = db.Values.Find(id);
            db.Values.Remove(values);
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
