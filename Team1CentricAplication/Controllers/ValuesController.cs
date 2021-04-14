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
using System.Net.Mail;

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
            //string notification = "Recognition sent to:<br/>";
            //var Profiles = from a in db.Profiles select a;
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
            /*if (ModelState.IsValid)
            {
                var firstName = 1.Profiles.firstName;
                var lastName = 1.Profiles.lastName;
                var email = 1.Profiles.email;
                var nomintedvalue = 1.Values.nominatedValue;
                var recognitionNote = 1.Values.recognitionNote;
                var msg = "Hi" + firstName + " " + lastName + ".\n\nCongrats!! You have been reconized for " + nomintedvalue + ". \n\n This is what they had to say about it! \n" + recognitionNote;
                MailMessage myMessage = new MailMessage();
                MailAddress from = new MailAddress("CentricRecognition@gmail.com", "SysAdmin");
                myMessage.From = from;
                myMessage.To.Add(email);
                myMessage.Subject = "You have been reconized";
                myMessage.Body = msg;
                try
                {
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("GmailUserAcnt", "Password");
                    smtp.EnableSsl = true;
                    smtp.Send(myMessage);
                    TempData["mailError"] = "";
                }
                catch
                {
                    //this captures an Exception and allows you to display the message in the View
                    TempData["mailError"] = ex.Message;
                    return View("mailError");
                }

            }

            ViewBag.notification = notification;*/

            ViewBag.profilesID = new SelectList(db.Profiles, "profilesID", "fullName", values.profilesID);
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
            ViewBag.profilesID = new SelectList(db.Profiles, "profilesID", "fullName", values.profilesID);
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
            ViewBag.profilesID = new SelectList(db.Profiles, "profilesID", "fullName", values.profilesID);
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
