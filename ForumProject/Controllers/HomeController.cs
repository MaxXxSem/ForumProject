using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForumProject.Models;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace ForumProject.Controllers
{
    public class HomeController : Controller
    {
        //Index page
        public ActionResult Index()
        {
            IEnumerable<Records> records;
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                records = entities.Records.Include(p => p.User).Include(p => p.UsersWhoLike).OrderByDescending(p => p.Date).ToList();

                if (Session["UserId"] != null)
                {
                    int userId = int.Parse(Session["UserId"].ToString());
                    ViewBag.LikedRecords = entities.Users.Find(userId).LikedRecords.ToList();       //mark like button by certain image
                }
            }

            return View(records);
        }

        //Sidebar partial view
        public ActionResult Sidebar()
        {
            IEnumerable<Topics> topics;
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                topics = entities.Topics.ToList();
                ViewBag.Topics = topics;
                ViewBag.PopularRecords = new Popular().MostPopularRecords();
            }

            return PartialView();
        }

        /*Search*/
        public ActionResult Search(string searchLine)
        {
            IEnumerable<Records> records;
            if (!searchLine.Equals(""))
            {
                using (ForumDBEntities entities = new ForumDBEntities())
                {
                    records = entities.Records.Include(r => r.User).Include(r => r.UsersWhoLike).Where(r => r.Text.Contains(searchLine)).OrderByDescending(r => r.Date).ToList();
                }
            }
            else
            {
                records = new List<Records>();
            }

            return View("~/Views/Home/Index.cshtml", records);
        }
    }
}