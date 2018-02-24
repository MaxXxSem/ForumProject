using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ForumProject.Models.ViewModels
{
    public class AddRecordViewModel
    {
        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [Required]
        [StringLength(1024)]
        public string Text { get; set; }

        public int SubtopicId { get; set; }
    }
}