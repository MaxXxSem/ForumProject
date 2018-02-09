using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForumProject.Models;
using System.Data.Entity;

namespace ForumProject.Controllers
{
    public class TopicsController : Controller
    {
        //Show subtopics in certain topics
        [HttpGet]
        public ActionResult Subtopics(int Id)
        {
            IEnumerable<Subtopics> sub;          //subtopic list
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                sub = entities.Subtopics.Where(e => e.Topics.Id == Id).ToList();
            }

            return View(sub);
        }

        //Show records in certain subtopic
        public ActionResult SubtopicsRecords(int Id)
        {
            IEnumerable<Records> records;
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                records = entities.Records.Include(r => r.Subtopic).Where(r => r.Subtopic.Id == Id).Include(r => r.User).Include(r => r.UsersWhoLike).ToList();
            }

            return View("~/Views/Home/Index.cshtml", records);
        }
    }
}