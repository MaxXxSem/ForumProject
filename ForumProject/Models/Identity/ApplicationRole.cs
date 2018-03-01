using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;

namespace ForumProject.Models.Identity
{
    public class ApplicationRole : IRole<Guid>
    {
        public ApplicationRole()
        {
            Id = Guid.NewGuid();
            Users = new HashSet<ApplicationUser>();
        }

        public ApplicationRole(string name) : this()
        {
            Name = name;
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(64)]
        public virtual string Name { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}