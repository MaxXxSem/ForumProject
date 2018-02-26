using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForumProject.Models;
using ForumProject.Models.Data;
using System.Data.Entity;
using ForumProject.Models.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNet.Identity;

namespace ForumProject.Controllers
{
    public class AuthController : Controller
    {
        //return authorization page
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        //check user login and pass
        [HttpPost]
        public ActionResult SignIn(string login, string password)
        {
            if (new SignInUp().TrySignIn(login, password))
            {
                using (ForumDBEntities entities = new ForumDBEntities())
                {
                    var user = entities.Users.Where(u => u.Login == login).First();
                    Session["UserId"] = user.Id;                                            //write Id into Session
                    Session["UserName"] = user.Name;

                    new Profile().CheckLevelInfo(entities, user.Id);
                }

                return Json(true);
            }

            return Json(false);
        }

        //sign out
        public ActionResult SignOut()
        {
            if (Session["UserId"] != null)
            {
                Session["UserId"] = null;
                Session["UserName"] = null;
            }

            return RedirectToAction("Index", "Home");
        }

        //returns SignUp page
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        //Add new user to DB
        [HttpPost]
        public async Task<ActionResult> SignUp(RegistrationModel model/*user*/)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser us = new ApplicationUser() {UserName = model.Login, PasswordHash = model.Password };

            var result = await manager.CreateAsync(us, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Such user already exists");
            }

            return View(model);
            //bool exists = false;

            //using (ForumDBEntities entities = new ForumDBEntities())
            //{
            //    if (new SignInUp().TrySignUp(user.Login))
            //    {
            //        entities.Users.Add(user);
            //        entities.SaveChanges();
            //    }
            //    else
            //    {
            //        exists = true;
            //    }
            //}

            //if (exists == false)
            //{
            //    return Json(true);
            //}
            //else
            //{
            //    return Json(false);
            //}
        }

        //delete profile
        [HttpPost]
        public ActionResult Delete()
        {
            if (Session["UserId"] != null)                                              //if user is autorized
            {
                var userId = int.Parse(Session["UserId"].ToString());
                using (ForumDBEntities entities = new ForumDBEntities())
                {
                    var user = entities.Users.Where(u => u.Id == userId).First();       //check user in database
                    if (user != null)
                    {
                        entities.Users.Remove(user);                                    //remove user from db
                        entities.SaveChanges();
                        Session["UserId"] = null;
                        Session["UserName"] = null;
                    }
                }

                return RedirectToAction("Index", "Home");                               //move to index page
            }
            else
            {
                return new HttpUnauthorizedResult("Unavailable operation");
            }
        }
    }
}