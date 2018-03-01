using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;

namespace ForumProject.Models.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() { }

        public new ICollection<ApplicationUser> Users { get; set; }
    }
}