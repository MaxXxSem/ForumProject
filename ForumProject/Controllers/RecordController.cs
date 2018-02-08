using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForumProject.Models;
using System.Data.Entity;

namespace ForumProject.Controllers
{
    public class RecordController : Controller
    {
        /*Show AddRecord view
        Id - Subtopic id
        */
        [HttpGet]
        public ActionResult AddRecord(int Id)
        {
            ViewBag.SubtopicId = Id;

            Users user = null;
            //identify user
            if (Session["UserId"] != null)
            {
                using (ForumDBEntities entities = new ForumDBEntities())
                {
                    var userId = int.Parse(Session["UserId"].ToString());
                    user = entities.Users.Where(u => u.Id == userId).ToList().First();
                }
            }

            return View(user);
        }

        /*Add record to database*/
        [HttpPost]
        public ActionResult AddRecord(Records record)
        {
            if (Session["UserId"] != null)
            {
                record.Date = DateTime.Now;
                record.UserId = int.Parse(Session["UserId"].ToString());

                using (ForumDBEntities entities = new ForumDBEntities())
                {
                    entities.Records.Add(record);
                    entities.SaveChanges();
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return new HttpUnauthorizedResult("Unavailable action");
            }
        }

        /*certain records view
        Id - record id 
        */
        [HttpGet]
        public ActionResult RecordView(int Id)
        {
            Records record;
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                record = entities.Records.Include(r => r.User).Where(r => r.Id == Id).ToList().First();     //ToList??
                ViewBag.Comments = entities.Comments.Include(c => c.User).Include(c => c.UsersWhoLike).Where(c => c.Record.Id == Id).OrderByDescending(c => c.Date).ToList();   //send comments to View

                if (Session["UserId"] != null)
                {
                    var userId = int.Parse(Session["UserId"].ToString());
                    var user = entities.Users.Find(userId);
                    ViewBag.LikedComments = user.LikedComments.ToList();

                    if (user.AccessLevel.Name.Equals("moderator"))
                    {
                        ViewBag.IsModerator = true;
                    }
                    else
                    {
                        ViewBag.IsModerator = false;
                    }
                }
            }

            return View(record);
        }

        [HttpPost]
        public ActionResult RecordView(Comments comment)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("SignIn", "Auth");
            }
            else
            {
                comment.Date = DateTime.Now;
                comment.UserId = int.Parse(Session["UserId"].ToString());

                using (ForumDBEntities entities = new ForumDBEntities())
                {
                    entities.Comments.Add(comment);
                    entities.SaveChanges();
                }

                return RecordView(comment.RecordId);
            }
        }

        /*id - record id*/
        public ActionResult DeleteRecord(int id)
        {
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                var user = entities.Records.Where(r => r.Id == id).First();
                entities.Records.Remove(user);
                entities.SaveChanges();
            }

            return new EmptyResult();
        }

        /*id - comment id*/
        public ActionResult DeleteComment(int id)
        {
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                entities.Comments.Remove(entities.Comments.Find(id));
                entities.SaveChanges();
            }

            return new EmptyResult();
        }
        
    }
}