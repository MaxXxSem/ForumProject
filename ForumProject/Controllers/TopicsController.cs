using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForumProject.Models;
using ForumProject.Models.Data;
using ForumProject.Models.ViewModels;
using System.Data.Entity;
using PagedList;
using PagedList.Mvc;

namespace ForumProject.Controllers
{
    public class TopicsController : Controller
    {
        //Show subtopics in certain topic
        [HttpGet]
        public ActionResult Subtopics(int Id)
        {
            IEnumerable<SubtopicViewModel> subtopics;                                             //subtopics list
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                subtopics = (from s in entities.Subtopics
                      where s.Topic.Id == Id
                      select new SubtopicViewModel { Id = s.Id, Name = s.Name }).ToList();
            }

            return View(subtopics);
        }

        //Show records in certain subtopic
        public ActionResult SubtopicsRecords(int Id, int? page)
        {
            int pageNum = page ?? 1;
            IEnumerable<RecordsListViewModel> records;
            ViewBag.PagedAction = PageInfo.PagedAction.SubtopicsRecords;            //choose action to call
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                records = (from r in entities.Records
                           where r.Subtopic.Id == Id
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

            return View("~/Views/Home/Index.cshtml", records);
        }
    }
}