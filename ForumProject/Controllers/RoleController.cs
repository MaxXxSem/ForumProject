using ForumProject.Models.Identity;
using ForumProject.Models.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ForumProject.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private ApplicationRoleManager RoleManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>(); }
        }

        public ActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRole(AddRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await RoleManager.CreateAsync(new ApplicationRole()
            {
                Name = model.Name
            });

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Can't add role");
            }

            return View(model);
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
                    Name = name
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
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Can't update role");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                    return RedirectToAction("Index", "Home");
                }
            }

            return new HttpStatusCodeResult(400, "Can't delete role");
        }
    }
}