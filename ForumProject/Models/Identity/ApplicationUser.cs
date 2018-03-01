using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using ForumProject.Models.Data;

namespace ForumProject.Models.Identity
{
    public class ApplicationUser : IUser
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();                     //New id
            Roles = new HashSet<ApplicationRole>();
        }

        public ApplicationUser(string userName) : this()
        {
            UserName = userName;
        }

        [Key]
        public virtual string Id { get; set; }

        [Required]
        [StringLength(64)]
        public virtual string UserName { get; set; }            //login

        [Required]
        public virtual string PasswordHash { get; set; }
        
        public virtual string SecurityStamp { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Users User { get; set; }

        public virtual ICollection<ApplicationRole> Roles { get; set; }
    }
}