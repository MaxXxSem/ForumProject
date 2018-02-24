using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ForumProject.Models.ViewModels
{
    public class SubtopicViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(32)]
        public string Name { get; set; }
    }
}