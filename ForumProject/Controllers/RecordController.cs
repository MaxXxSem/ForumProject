using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForumProject.Models.ViewModels;
using ForumProject.Models.Data;
using ForumProject.Models.DTO;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace ForumProject.Controllers
{
    public class RecordController : Controller
    {
        /*Show AddRecord view
        Id - Subtopic id
        */
        [HttpGet]
        [Authorize]
        public ActionResult AddRecord(int Id)
        {
            ViewBag.SubtopicId = Id;
            UserAddsRecordViewModel user = new UserAddsRecordViewModel();

            using (ForumDBEntities entities = new ForumDBEntities())
            {
                var userId = entities.UserData.Find(User.Identity.GetUserId()).User.Id;
                user.Id = userId;
                user.MainPhoto = entities.Users.Where(u => u.Id == userId).Select(u => u.MainPhoto).First();
            }

            return View(user);
        }

        /*Add record to database*/
        [HttpPost]
        [Authorize]
        public ActionResult AddRecord(AddRecordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(400, "Wrong data");
            }
        
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                Records record = new Records()
                {
                    Name = model.Name,
                    Text = model.Text,
                    SubtopicId = model.SubtopicId,
                    Date = DateTime.Now,
                    UserId = entities.UserData.Find(User.Identity.GetUserId()).User.Id
                };

                entities.Records.Add(record);
                entities.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
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
                viewRecord = (from r in entities.Records
                              where r.Id == Id
                              select new RecordViewModel()
                              {
                                  Id = r.Id,
                                  UserId = r.UserId,
                                  Name = r.Name,
                                  Text = r.Text,
                                  Date = r.Date,
                                  User = r.User,
                                  Comments = (from c in r.Comments
                                              where c.Record.Id == Id
                                              select new CommentDTO()                                   //use DTO to convey to controller
                                              {
                                                  Id = c.Id,
                                                  Date = c.Date,
                                                  Text = c.Text,
                                                  UserId = c.UserId,
                                                  UserName = c.User.Name,
                                                  UserMainPhoto = c.User.MainPhoto,
                                                  UsersWhoLikeCount = c.UsersWhoLike.Count
                                              }).ToList()
                              }).First();

                if (User.Identity.IsAuthenticated)
                { 
                    var user = entities.UserData.Find(User.Identity.GetUserId()).User;
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
        [Authorize]
        public ActionResult AddComment(AddCommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(400, "Wrong input");
            }

            Comments comment;
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                comment = new Comments()
                {
                    Text = model.Text,
                    RecordId = model.RecordId,
                    Date = DateTime.Now,
                    UserId = entities.UserData.Find(User.Identity.GetUserId()).User.Id
                };

                entities.Comments.Add(comment);
                entities.SaveChanges();
            }

            return RedirectToAction("RecordView", "Record", comment.RecordId);      //to avoid resending form after page reloading
        }

        /* Delete record
         * id - record id
         */
        [Authorize]
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
        [Authorize]
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