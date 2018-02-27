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
using Microsoft.Owin.Security;
using System.Security.Claims;

namespace ForumProject.Controllers
{
    public class AuthController : Controller
    {
        //object for interacting with users
        private ApplicationUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }

        //for SignIn / SignOut management
        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; } 
        }

        //return authorization page
        [HttpGet]
        public ActionResult SignIn(string returnUrl)                                //-----------------------------------------
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //check user login and pass
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignIn(Login model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(model.UserName, model.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "Wrong username or password");
                }
                //else if(ProfileInfo.IsBlocked(model.UserName))                  //check if user is blocked
                //{
                //    ModelState.AddModelError("", "Your account is blocked");
                //}
                else
                {
                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignOut();
                    //IsPersistent - to save authentication after browser closing
                    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, claim);
                    
                    int id = UserManager.FindByName(model.UserName).User.Id;        //get current user's id
                    ProfileInfo.CheckLevelInfo(id);                                     //try to change forum level

                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //sign out
        [Authorize]
        public ActionResult SignOut()
        {
            AuthenticationManager.SignOut();
            Session.Abandon();
            return RedirectToAction("SignIn");
        }

        //returns SignUp page
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        //Add new user to DB
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignUp(RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            Users userData = new Users()
            {
                Name = model.Name,
                Login = model.Name,
                Password = model.Password
            };
            ApplicationUser user = new ApplicationUser() {UserName = model.Login, PasswordHash = model.Password, User = userData };
            var result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("SignIn");
            }
            else
            {
                ModelState.AddModelError("", "Such user already exists");
            }

            return View(model);
        }

        //delete profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Delete()
        {
            ApplicationUser user = await UserManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                var result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("SignOut");
                }
            }

            return RedirectToAction("Index", "Home");                   //------------------------------------------
        }
    }
}