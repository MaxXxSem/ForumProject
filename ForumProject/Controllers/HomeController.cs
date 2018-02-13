using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForumProject.Models;
using System.Data.Entity;
using System.Threading.Tasks;
using PagedList;
using PagedList.Mvc;

namespace ForumProject.Controllers
{
    public class HomeController : Controller
    {
        //Index page with all records list
        public ActionResult Index(int? page)
        {
            int pageNum = page ?? 1;
            ViewBag.PagedAction = PageInfo.PagedAction.Index;                   //choose action to call
            IPagedList<Records> records;
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                records = entities.Records.Include(p => p.User).Include(p => p.UsersWhoLike).OrderByDescending(p => p.Date).ToPagedList(pageNum, PageInfo.pageSize);

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

        //Search by keyword
        public ActionResult Search(string searchLine, int? page)
        {
            int pageNum = page ?? 1;
            IEnumerable<Records> records;
            ViewBag.PagedAction = PageInfo.PagedAction.Search;             //choose action to call
            ViewBag.SearchLine = searchLine;
            if (!searchLine.Equals(""))
            {
                using (ForumDBEntities entities = new ForumDBEntities())
                {
                    records = entities.Records.Include(r => r.User).Include(r => r.UsersWhoLike).Where(r => r.Text.Contains(searchLine)).OrderByDescending(r => r.Date).ToPagedList(pageNum, PageInfo.pageSize);
                }
            }
            else
            {
                records = new List<Records>();          //??????????????
            }

            return View("~/Views/Home/Index.cshtml", records);
        }
    }
}