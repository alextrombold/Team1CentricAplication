using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Team1CentricAplication.DAL;
using Team1CentricAplication.Models;

namespace Team1CentricAplication.Controllers
{
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
            Profile = db.Profiles.Include(p => p.AwardRecipient).OrderBy(r => r.lastName).ThenBy(r => r.firstName); ;
            // check to see if a search was requested and do it
            if (!String.IsNullOrEmpty(searchString))
            {
                Profile = Profile.Where(r => r.lastName.Contains(searchString) || r.firstName.Contains(searchString));
            }
            var profileList = Profile.ToPagedList(pageNumber, pgSize);
            ViewBag.search = String.IsNullOrEmpty(searchString) ? "" : searchString;

            var awards = db.Values.Where(a => a.AwardNominee == a.AwardRecipient);
            ViewBag.award = awards.ToList();
            return View(profileList);
        }

        // GET: Profiles/Details/5
        public ActionResult ProfilesAndValues()
        {
            var profilesList = db.Profiles.Include(o => o.AwardRecipient).ToList();
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
        public ActionResult Create([Bind(Include = "profilesID,firstName,lastName,phone,email,city,zip,role,profilePicture")] Profiles profiles)
        {
            if (ModelState.IsValid)
            {
                Guid profilesID;
                Guid.TryParse(User.Identity.GetUserId(), out profilesID);
                profiles.profilesID = profilesID;
                profiles.role = Profiles.roles.employee;

                HttpPostedFileBase file = Request.Files["UploadedImage"];
                if (file != null && file.FileName != null && file.FileName != "")
                {
                    FileInfo fi = new FileInfo(file.FileName);
                    if (fi.Extension != ".png" && fi.Extension != ".PNG" && fi.Extension != ".jpg" && fi.Extension != ".gif")
                    {
                        ViewBag.Errormsg = "The file, " + file.FileName + ", does not have a valid image extension.";
                        return View(profiles);
                    }
                    else
                    {
                        profiles.profilePicture = Guid.NewGuid().ToString() + fi.Extension;
                        file.SaveAs(Server.MapPath("~/Images/" + profiles.profilePicture));

                    }

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
                Profiles currentProfile = db.Profiles.Find(id);
                TempData["oldPhoto"] = currentProfile.profilePicture;

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
        public ActionResult Edit([Bind(Include = "profilesID,firstName,lastName,phone,email,city,zip,role,profilePicture")] Profiles profiles)
        {
            if (ModelState.IsValid)
            {
                string removeImage = "";
                if (Request.Form["removeImage"] != null)
                {
                    removeImage = Request.Form["removeImage"];
                }

                HttpPostedFileBase file = Request.Files["UploadedImage"];

                if (file != null && file.FileName != null && file.FileName != "")
                {
                    FileInfo fi = new FileInfo(file.FileName);
                    if (fi.Extension != ".png" && fi.Extension != ".PNG"  && fi.Extension != ".jpg" && fi.Extension != ".gif")
                    {
                        ViewBag.Errormsg = "Image File Extension is not valid.";
                        return View(profiles);
                    }
                    else
                    {
                        string path = Server.MapPath("~/Uploads/" + TempData["oldPhoto"].ToString());
                        try
                        {
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            else
                            {
                                // no file
                            }
                        }
                        catch (Exception Ex)
                        {
                            ViewBag.deleteFailed = Ex.Message;
                            return View("DeleteFailed");
                        }
                        if (fi.Name != null && fi.Name != "")
                        {
                            profiles.profilePicture = Guid.NewGuid().ToString() + fi.Extension;
                            file.SaveAs(Server.MapPath("~/Images/" + profiles.profilePicture));
                        }
                    }
                }
                else
                {
                    if (TempData["oldPhoto"] != null)
                    {
                        if (removeImage=="Remove") 
                        {
                            profiles.profilePicture = "";
                            string path = Server.MapPath("~/Images/" + TempData["oldPhoto"].ToString());
                            try
                            {
                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }
                                else
                                {
                                    // no file
                                }
                            }
                            catch (Exception Ex)
                            {
                                ViewBag.deleteFailed = Ex.Message;
                                return View("DeleteFailed");
                            }

                        }
                    else
                        {
                            profiles.profilePicture = TempData["oldPhoto"].ToString();
                        }
                    } 
                }
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
            string imageName = profiles.profilePicture;
            string path = Server.MapPath("~/Images/" + imageName);
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                else
                {
                    // no file
                }
            }
            catch (Exception Ex)
            {
                ViewBag.deleteFiled = Ex.Message;
                return View("DeleteFailed");
            }

            
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
