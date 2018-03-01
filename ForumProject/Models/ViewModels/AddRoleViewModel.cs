using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ForumProject.Models.ViewModels
{
    public class AddRoleViewModel
    {
        [Required]
        [StringLength(64)]
        public string Name { get; set; }
    }
}