using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForumProject.Models;
using ForumProject.Models.Data;
using ForumProject.Models.ViewModels;
using ForumProject.Models.DTO;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using ForumProject.Models.Identity;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace ForumProject.Controllers
{
    public class ProfileController : Controller
    {
        /*Show profile page
        Id - publisher id
        */
        public async Task<ActionResult> ProfileView(int Id)
        {
            //CHECK GUEST IN AddRecordbtn
            int userId = 0;                                 //get user id
            ProfileViewModel profile;

            using (ForumDBEntities entities = new ForumDBEntities())
            {
                ViewBag.IsOwner = false;
                if (User.Identity.IsAuthenticated)              //if authorized user
                {
                    userId = entities.UserData.Find(User.Identity.GetUserId()).User.Id;
                    if (userId == Id)                           //if user's profile page
                    {
                        ViewBag.IsOwner = true;
                    }
                }

                profile = await (from u in entities.Users
                           where u.Id == Id
                           select new ProfileViewModel()
                           {
                               Id = u.Id,
                               Name = u.Name,
                               MainPhoto = u.MainPhoto,
                               Records = (from r in u.Records
                                          select new ProfileRecordDTO()
                                          {
                                              Id = r.Id, Name = r.Name, Text = r.Text, Date = r.Date, UsersWhoLikeCount = r.UsersWhoLike.Count
                                          }).ToList(),
                               LikedRecords = (from r in u.LikedRecords
                                               select new ProfileRecordDTO()
                                               {
                                                   Id = r.Id, Name = r.Name, Text = r.Text, Date = r.Date, UsersWhoLikeCount = r.UsersWhoLike.Count
                                               }).ToList(),
                               Subscriptions = (from s in u.Subscriptions
                                                select new SubscribtionsDTO()
                                                {
                                                    Id = s.Id, Name = s.Name, MainPhoto = s.MainPhoto, LevelInfoName = s.LevelInfo.Name
                                                }).ToList(),
                               Subscribers = (from s in u.Subscribers
                                              select new SubscribtionsDTO()
                                              {
                                                  Id = s.Id, Name = s.Name, MainPhoto = s.MainPhoto, LevelInfoName = s.LevelInfo.Name
                                              }).ToList()
                           }).FirstAsync();

                //check user in subscriptions
                if (User.Identity.IsAuthenticated && entities.Users.Find(userId).Subscriptions.Any(u => u.Id == Id))
                {
                    ViewBag.IsSubscription = true;
                }
                else
                {
                    ViewBag.IsSubscription = false;
                }
            }

            return View(profile);
        }

        //delete profile
        [HttpPost]
        [Authorize]
        public ActionResult Delete()
        {
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                ViewBag.UserId = entities.UserData.Find(User.Identity.GetUserId()).User.Id;
            }

            return View();
        }

        //change user's image
        [HttpPost]
        [Authorize]
        public ActionResult UploadImage(HttpPostedFileBase image)
        {
            int userId = 0;

            if (image != null)
            {
                using (ForumDBEntities entities = new ForumDBEntities())
                {
                    userId = entities.UserData.Find(User.Identity.GetUserId()).User.Id;
                    var user = entities.Users.Where(u => u.Id == userId).ToList().First();
                    string name = System.IO.Path.GetFileName(image.FileName);
                    name = name.Substring(name.LastIndexOf('.'));
                    image.SaveAs(Server.MapPath("~/Content/images/" + user.Id + name));
                    user.MainPhoto = user.Id + name;
                    entities.SaveChanges();
                }
            }

            return RedirectToAction("ProfileView", new { id = userId });
        }

        //changes user name
        [HttpPost]
        [Authorize]
        public ActionResult ChangeName(int id, string name)
        {
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                var userId = entities.UserData.Find(User.Identity.GetUserId()).User.Id;
                ViewBag.Name = entities.Users.Find(id).Name;
                //ИДЕТ ДВА ЗАПРОСА ПОДРЯД
                bool req = Request.IsAjaxRequest();
                if (name != null)
                {
                    entities.Users.Find(id).Name = name;
                    entities.SaveChanges();
                    ViewBag.Name = name;
                }
            }

            return Content(name);                   //return name
        }

        /*Subscribe
         * id - subscription id
         */
        [Authorize]
        public ActionResult Subscribe(int id)
        {
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                var userId = entities.UserData.Find(User.Identity.GetUserId()).User.Id;
                entities.Users.Find(userId).Subscriptions.Add(entities.Users.Find(id));
                entities.SaveChanges();
            }

            ViewBag.UserId = id;                //AJAX can't load Model immediately
            return PartialView();
        }

        /*Unsubscribe
         * id - subscription id
         */
        [Authorize]
        public ActionResult Unsubscribe(int id)
        {
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                var userId = entities.UserData.Find(User.Identity.GetUserId()).User.Id;
                entities.Users.Find(userId).Subscriptions.Remove(entities.Users.Find(id));
                entities.SaveChanges();
            }

            ViewBag.UserId = id;                //AJAX can't load Model immediately
            return PartialView();
        }

        /*like record
         * id - record id
        */
        [Authorize]
        public ActionResult LikeRecord(int id)
        {
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                var userId = entities.UserData.Find(User.Identity.GetUserId()).User.Id;
                entities.Users.Find(userId).LikedRecords.Add(entities.Records.Find(id));
                entities.SaveChanges();
            }

            return new EmptyResult();
        }

        /*unlike record
         * id - record id
        */
        [Authorize]
        public ActionResult UnlikeRecord(int id)
        {
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                var userId = entities.UserData.Find(User.Identity.GetUserId()).User.Id;
                entities.Users.Find(userId).LikedRecords.Remove(entities.Records.Find(id));
                entities.SaveChanges();
            }

            return new EmptyResult();
        }

        /* Like comment
         * id - comment id
         */
        [Authorize]
        public ActionResult LikeComment(int id)
        {
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                var userId = entities.UserData.Find(User.Identity.GetUserId()).User.Id;
                entities.Users.Find(userId).LikedComments.Add(entities.Comments.Find(id));
                entities.SaveChanges();
            }

            return new EmptyResult();
        }

        /*unlike comment
         * id - record id
        */
        [Authorize]
        public ActionResult UnlikeComment(int id)
        {
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                var userId = entities.UserData.Find(User.Identity.GetUserId()).User.Id;
                entities.Users.Find(userId).LikedComments.Remove(entities.Comments.Find(id));
                entities.SaveChanges();
            }

            return new EmptyResult();
        }

        //for getting id from layout page
        public int GetId()
        {
            int id = 0;
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                id = entities.UserData.Find(User.Identity.GetUserId()).User.Id;
            }

            return id;
        }

        //for getting name from layout page
        public string GetName()
        {
            string name;
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                name = entities.UserData.Find(User.Identity.GetUserId()).User.Name;
            }

            return name;
        }
    }
}