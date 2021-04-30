using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team1CentricAplication.DAL;
using Team1CentricAplication.Models;
using System.Data;
using System.Data.Entity;
using System.Net;
using Microsoft.AspNet.Identity;


namespace Team1CentricAplication.Controllers
{
    public class HomeController : Controller
    {
        private Team1Context db = new Team1Context();
        public ActionResult Index()
        {
            var values = db.Values;
            var count = db.Profiles.Include(p => p.AwardRecipient).Count();
          
            //var top10 = (from v in valuesList
            //             group v by v.AwardNominee into g
            //             let count = g.Count()
            //             orderby count descending
            //             select new { Value = g.Key, Count = count }).Take(10);
            var top10 = db.Profiles.Include(p => p.AwardRecipient).OrderByDescending(p => p.AwardRecipient.Count()).Take(10);
            //var Excellence10 = (from v in valuesList
            //                    where v.nominatedValues == Values.CoreValue.Excellence
            //                    group v by v.AwardNominee into g
            //                    let count = g.Count()
            //                    orderby count descending
            //                    select new { Value = g.Key, Count = count }).Take(10);
            var Excellence10 = db.Values.Where(v => v.nominatedValues == Values.CoreValue.Excellence).Include(v => v.AwardNominee).OrderByDescending(o => o.recognizationDate).Take(10);

            //var Integrity10 = (from v in valuesList
            //                 where v.nominatedValues == Values.CoreValue.Integrity
            //               group v by v.AwardNominee into g
            //             let count = g.Count()
            //           orderby count descending
            //         select new { Value = g.Key, Count = count }).Take(10);
            var Integrity10 = db.Values.Where(v => v.nominatedValues == Values.CoreValue.Integrity).Include(v => v.AwardNominee).OrderByDescending(o => o.recognizationDate).Take(10);
            //var Stewardship10 = (from v in valuesList
            //                   where v.nominatedValues == Values.CoreValue.Stewardship
            //                 group v by v.AwardNominee into g
            //               let count = g.Count()
            //             orderby count descending
            //           select new { Value = g.Key, Count = count }).Take(10);
            var Stewardship10 = db.Values.Where(v => v.nominatedValues == Values.CoreValue.Stewardship).Include(v => v.AwardNominee).OrderByDescending(o => o.recognizationDate).Take(10);
            //var Innovate10 = (from v in valuesList
            //                where v.nominatedValues == Values.CoreValue.Innovate
            //              group v by v.AwardNominee into g
            //            let count = g.Count()
            //          orderby count descending
            //        select new { Value = g.Key, Count = count }).Take(10);
            var Innovate10 = db.Values.Where(v => v.nominatedValues == Values.CoreValue.Innovate).Include(v => v.AwardNominee).OrderByDescending(o => o.recognizationDate).Take(10);
            //var Balance10 = (from v in valuesList
            //               where v.nominatedValues == Values.CoreValue.Balance
            //             group v by v.AwardNominee into g
            //           let count = g.Count()
            //         orderby count descending
            //       select new { Value = g.Key, Count = count }).Take(10);
            var Balance10 = db.Values.Where(v => v.nominatedValues == Values.CoreValue.Balance).Include(v => v.AwardNominee).OrderByDescending(o => o.recognizationDate).Take(10);
            var Greater10 = db.Values.Where(v => v.nominatedValues == Values.CoreValue.GreaterGood).Include(v => v.AwardNominee).OrderByDescending(o => o.recognizationDate).Take(10);
            var Culture10 = db.Values.Where(v => v.nominatedValues == Values.CoreValue.Culture).Include(v => v.AwardNominee).OrderByDescending(o => o.recognizationDate).Take(10);


            ViewBag.top10 = top10.ToList();
            ViewBag.excellence = Excellence10.ToList();
            ViewBag.integrity = Integrity10.ToList();
            ViewBag.stewardship = Stewardship10.ToList();
            ViewBag.innovate = Innovate10.ToList();
            ViewBag.balance = Balance10.ToList();
            ViewBag.greater = Greater10.ToList();
            ViewBag.culture = Culture10.ToList();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}