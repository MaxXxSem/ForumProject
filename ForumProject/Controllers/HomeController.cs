using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForumProject.Models;
using ForumProject.Models.Data;
using ForumProject.Models.ViewModels;
using System.Data.Entity;
using System.Threading.Tasks;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNet.Identity.Owin;
using ForumProject.Models.Identity;
using Microsoft.AspNet.Identity;

namespace ForumProject.Controllers
{
    public class HomeController : Controller
    {
        //Index page with all records list
        public ActionResult Index(int? page)
        {
            int pageNum = page ?? 1;
            ViewBag.PagedAction = PageInfo.PagedAction.Index;                   //choose action to call

            IPagedList<RecordsListViewModel> records;
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                records = (from r in entities.Records
                           select new RecordsListViewModel()
                           {
                               Id = r.Id,
                               Name = r.Name,
                               Text = r.Text,
                               Date = r.Date,
                               UserId = r.UserId,
                               SubtopicId = r.SubtopicId,
                               User = r.User,
                               UsersWhoLike = r.UsersWhoLike
                           }).OrderByDescending(r => r.Date).ToPagedList(pageNum, PageInfo.pageSize);

                if (User.Identity.IsAuthenticated)
                {
                    int userId = entities.UserData.Find(User.Identity.GetUserId()).User.Id;                //get user id
                    ViewBag.LikedRecords = (from r in entities.Users.Find(userId).LikedRecords                  //get liked by user records list
                                            select new RecordsListViewModel()
                                            {
                                                Id = r.Id
                                            }).ToList();       //mark like button by certain image
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
                ViewBag.PopularRecords = Popular.MostPopularRecords();
            }

            return PartialView();
        }

        //Search by keyword
        public ActionResult Search(string searchLine, int? page)
        {
            int pageNum = page ?? 1;
            ViewBag.PagedAction = PageInfo.PagedAction.Search;              //choose action to call
            ViewBag.SearchLine = searchLine;                                //for pagination

            IEnumerable<RecordsListViewModel> records;

            if (!searchLine.Equals(""))
            {
                using (ForumDBEntities entities = new ForumDBEntities())
                {
                    records = (from r in entities.Records
                               where r.Text.Contains(searchLine)
                               select new RecordsListViewModel()
                               {
                                   Id = r.Id,
                                   Name = r.Name,
                                   Text = r.Text,
                                   Date = r.Date,
                                   UserId = r.UserId,
                                   SubtopicId = r.SubtopicId,
                                   User = r.User,
                                   UsersWhoLike = r.UsersWhoLike
                               }).OrderByDescending(r => r.Date).ToPagedList(pageNum, PageInfo.pageSize);
                }
            }
            else
            {
                return RedirectToAction("Index");
            }

            return View("~/Views/Home/Index.cshtml", records);
        }
    }
}