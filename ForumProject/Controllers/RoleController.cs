using ForumProject.Models.Identity;
using ForumProject.Models.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ForumProject.Models.Data;

namespace ForumProject.Controllers
{
    [Authorize(Roles = "admin")]
    public class RoleController : Controller
    {
        private ApplicationRoleManager RoleManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>(); }
        }

        private ApplicationUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }

        //start page
        public ActionResult Index()
        {
            IEnumerable<ApplicationRole> roles;
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                roles = entities.Roles.ToList();
            }

            return View(roles);
        }

        [HttpPost]
        public async Task<ActionResult> AddRole(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return new HttpStatusCodeResult(400, "Wrong data");
            }

            var role = new ApplicationRole() { Name = name };
            var result = await RoleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return new HttpStatusCodeResult(400, "Can't add role");
        }

        public async Task<ActionResult> EditRole(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return new HttpStatusCodeResult(400, "Wrong data");
            }

            var role = RoleManager.FindByNameAsync(name);
            if (role != null)
            {
                var edit = new EditRoleViewModel()
                {
                    Id = role.Result.Id,
                    Name = role.Result.Name
                };

                return View(edit);
            }

            return new HttpStatusCodeResult(400, "Wrong data");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditRole(EditRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await RoleManager.UpdateAsync(new ApplicationRole()
            {
                Id = model.Id,
                Name = model.Name
            });

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Can't update role");
            }

            return View(model);
        }

        public async Task<ActionResult> DeleteRole(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return new HttpStatusCodeResult(400, "Wrong data");
            }

            var role = await RoleManager.FindByNameAsync(name);
            if (role != null)
            {
                var result = await RoleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return new HttpStatusCodeResult(400, "Can't delete role");
        }

        //Add user to role
        [HttpPost]
        public async Task<ActionResult> AddToRole(string userName, string roleName)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(roleName))
            {
                return new HttpStatusCodeResult(400, "Wrond data");
            }

            ApplicationUser user = await UserManager.FindByNameAsync(userName);
            ApplicationRole role = await RoleManager.FindByNameAsync(roleName);

            if (user != null && role != null)
            {
                var result = await UserManager.AddToRoleAsync(user.Id, role.Name);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return new HttpStatusCodeResult(400, "Operation can't be performed");
        }

        //remove user from role
        [HttpPost]
        public async Task<ActionResult> RemoveFromRole(string userName, string roleName)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(roleName))
            {
                return new HttpStatusCodeResult(400, "Wrong data");
            }

            var user = await UserManager.FindByNameAsync(userName);
            var role = await RoleManager.FindByNameAsync(roleName);

            if (user != null && role != null)
            {
                var result = await UserManager.RemoveFromRoleAsync(user.Id, role.Name);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return new HttpStatusCodeResult(400, "Operation can't be performed");
        }
    }
}