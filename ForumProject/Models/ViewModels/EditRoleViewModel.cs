using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ForumProject.Models.ViewModels
{
    public class EditRoleViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(64)]
        public virtual string Name { get; set; }
    }
}