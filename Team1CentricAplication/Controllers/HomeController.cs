using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team1CentricAplication.DAL;
using Team1CentricAplication.Models;

namespace Team1CentricAplication.Controllers
{
    public class HomeController : Controller
    {
        private Team1Context db = new Team1Context();
        public ActionResult Index()
        {
            var values = db.Values;
            IList<Values> valuesList = values.ToList();
            var top10 = (from v in valuesList
                         group v by v.AwardNominee into g
                         let count = g.Count()
                         orderby count descending
                         select new { Value = g.Key, Count = count }).Take(10);
            var Excellence10 = (from v in valuesList
                                where v.nominatedValues == Values.CoreValue.Excellence
                                group v by v.AwardNominee into g
                                let count = g.Count()
                                orderby count descending
                                select new { Value = g.Key, Count = count }).Take(10);
            var Integrity10 = (from v in valuesList
                               where v.nominatedValues == Values.CoreValue.Integrity
                               group v by v.AwardNominee into g
                               let count = g.Count()
                               orderby count descending
                               select new { Value = g.Key, Count = count }).Take(10);
            var Stewardship10 = (from v in valuesList
                                 where v.nominatedValues == Values.CoreValue.Stewardship
                                 group v by v.AwardNominee into g
                                 let count = g.Count()
                                 orderby count descending
                                 select new { Value = g.Key, Count = count }).Take(10);
            var Innovate10 = (from v in valuesList
                              where v.nominatedValues == Values.CoreValue.Innovate
                              group v by v.AwardNominee into g
                              let count = g.Count()
                              orderby count descending
                              select new { Value = g.Key, Count = count }).Take(10);
            var Balance10 = (from v in valuesList
                             where v.nominatedValues == Values.CoreValue.Balance
                             group v by v.AwardNominee into g
                             let count = g.Count()
                             orderby count descending
                             select new { Value = g.Key, Count = count }).Take(10);

            ViewBag.top10 = top10;
            ViewBag.excellence = Excellence10;
            ViewBag.integrity = Integrity10;
            ViewBag.stewardship = Stewardship10;
            ViewBag.innovate = Innovate10;
            ViewBag.balance = Balance10;
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