using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForumProject.Models.ViewModels;
using ForumProject.Models.Data;
using System.Data.Entity;
using AutoMapper;

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
            UserAddsRecordViewModel user = new UserAddsRecordViewModel();

            //identify user
            if (Session["UserId"] == null)
            {
                return RedirectToAction("SignIn", "Auth");
            }
            else
            {
                using (ForumDBEntities entities = new ForumDBEntities())
                {
                    var userId = int.Parse(Session["UserId"].ToString());
                    user.MainPhoto = entities.Users.Where(u => u.Id == userId).Select(u => u.MainPhoto).First();
                }
            }

            return View(user);
        }

        /*Add record to database*/
        [HttpPost]
        public ActionResult AddRecord(AddRecordViewModel model)
        {
            if (Session["UserId"] != null)
            {
                Records record = new Records()
                {
                    Name = model.Name,
                    Text = model.Text,
                    SubtopicId = model.SubtopicId,
                    Date = DateTime.Now,
                    UserId = int.Parse(Session["UserId"].ToString())
                };

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
            RecordViewModel viewRecord;
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                var record = entities.Records.Include(r => r.User).Where(r => r.Id == Id).First();
                var comments = entities.Comments.Include(c => c.User).Include(c => c.UsersWhoLike).Where(c => c.Record.Id == Id).OrderByDescending(c => c.Date).ToList();   //send comments to View



                record.Comments = comments ?? null;

                Mapper.Initialize(cfg => cfg.CreateMap<Records, RecordViewModel>());
                viewRecord = Mapper.Map<RecordViewModel>(record);
                Mapper.Reset();

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

            return View(viewRecord);
        }

        //Add comment
        [HttpPost]
        [ActionName("RecordView")]
        public ActionResult AddComment(AddCommentViewModel model)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("SignIn", "Auth");
            }
            else
            {
                Comments comment = new Comments()
                {
                    Text = model.Text,
                    RecordId = model.RecordId,
                    Date = DateTime.Now,
                    UserId = int.Parse(Session["UserId"].ToString())
                };

                using (ForumDBEntities entities = new ForumDBEntities())
                {
                    entities.Comments.Add(comment);
                    entities.SaveChanges();
                }

                return RedirectToAction("RecordView", "Record", comment.RecordId);      //to avoid resending form after page reloading
            }
        }

        /* Delete record
         * id - record id
         */
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

        /* Delete comment
         * id - comment id
         */
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