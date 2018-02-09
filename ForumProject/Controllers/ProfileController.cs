﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForumProject.Models;
using System.Data.Entity;

namespace ForumProject.Controllers
{
    public class ProfileController : Controller
    {
        /*Show profile page
        Id - publisher id
        */
        public ActionResult ProfileView(int Id)
        {
            //CHECK GUEST IN AddRecordbtn
            var userId = Session["UserId"];
            if (userId != null)                             //if authorized user
            {
                if (int.Parse(userId.ToString()) == Id)     //if user's profile page
                {
                    ViewBag.UserStatus = "owner";
                }
                else                                        //if not user's page
                {
                    ViewBag.UserStatus = "user";
                }
            }
            else                                            //if guest
            {
                ViewBag.UserStatus = "guest";
            }

            Users user;
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                user = entities.Users.Where(u => u.Id == Id).Include(u => u.Records.Select(r => r.UsersWhoLike))
                    .Include(u => u.LikedRecords.Select(r => r.UsersWhoLike))
                    .Include(u => u.Subscriptions.Select(s => s.LevelInfo))
                    .Include(u => u.Subscribers.Select(s => s.LevelInfo)).ToList().First();

                //check user in subscriptions
                if (ViewBag.UserStatus.Equals("user") && userId != null && entities.Users.Find(userId).Subscriptions.Any(u => u.Id == Id))
                {
                    ViewBag.IsSubscription = true;
                }
                else
                {
                    ViewBag.IsSubscription = false;
                }
            }

            return View(user);
        }

        //delete profile
        [HttpPost]
        public ActionResult Delete()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.UserId = int.Parse(Session["UserId"].ToString());
                return View();
            }
            else
            {
                return new HttpUnauthorizedResult("Unavailable operation");
            }
        }

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase image)
        {
            var userId = int.Parse(Session["UserId"].ToString());

            if (image != null)
            {
                using (ForumDBEntities entities = new ForumDBEntities())
                {
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
        public ActionResult ChangeName(int id, string name)
        {
            using (ForumDBEntities entities = new ForumDBEntities())
            {
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
        public ActionResult Subscribe(int id)
        {
            var userId = int.Parse(Session["UserId"].ToString());
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                entities.Users.Find(userId).Subscriptions.Add(entities.Users.Find(id));
                entities.SaveChanges();
            }

            ViewBag.UserId = id;                //AJAX can't load Model immediately
            return PartialView();
        }

        /*Unsubscribe
         * id - subscription id
         */
        public ActionResult Unsubscribe(int id)
        {
            var userId = int.Parse(Session["UserId"].ToString());
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                entities.Users.Find(userId).Subscriptions.Remove(entities.Users.Find(id));
                entities.SaveChanges();
            }

            ViewBag.UserId = id;                //AJAX can't load Model immediately
            return PartialView();
        }

        /*like record
         * id - record id
        */
        public ActionResult LikeRecord(int id)
        {
            var userId = Session["UserId"];
            if (userId != null)
            {
                using (ForumDBEntities entities = new ForumDBEntities())
                {
                    int uId = int.Parse(userId.ToString());
                    entities.Users.Find(uId).LikedRecords.Add(entities.Records.Find(id));
                    entities.SaveChanges();
                }

                return new EmptyResult();
            }
            else
            {
                return RedirectToAction("SignIn", "Auth");
            }
        }

        /*unlike record
         * id - record id
        */
        public ActionResult UnlikeRecord(int id)
        {
            var userId = Session["UserId"];
            if (userId != null)
            {
                using (ForumDBEntities entities = new ForumDBEntities())
                {
                    int uId = int.Parse(userId.ToString());
                    entities.Users.Find(uId).LikedRecords.Remove(entities.Records.Find(id));
                    entities.SaveChanges();
                }

                return new EmptyResult();
            }
            else
            {
                return RedirectToAction("SignIn", "Auth");
            }
        }

        /*id - comment id*/
        public ActionResult LikeComment(int id)
        {
            var userId = Session["UserId"];
            if (userId != null)
            {
                using (ForumDBEntities entities = new ForumDBEntities())
                {
                    int uId = int.Parse(userId.ToString());
                    entities.Users.Find(uId).LikedComments.Add(entities.Comments.Find(id));
                    entities.SaveChanges();
                }

                return new EmptyResult();
            }
            else
            {
                return RedirectToAction("SignIn", "Auth");
            }
        }

        /*unlike comment
         * id - record id
        */
        public ActionResult UnlikeComment(int id)
        {
            var userId = Session["UserId"];
            if (userId != null)
            {
                using (ForumDBEntities entities = new ForumDBEntities())
                {
                    int uId = int.Parse(userId.ToString());
                    entities.Users.Find(uId).LikedComments.Remove(entities.Comments.Find(id));
                    entities.SaveChanges();
                }

                return new EmptyResult();
            }
            else
            {
                return RedirectToAction("SignIn", "Auth");
            }
        }
    }
}