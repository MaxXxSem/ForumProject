using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ForumProject.Models.ViewModels
{
    public class AddCommentViewModel
    {
        [Required]
        [StringLength(128)]
        public string Text { get; set; }

        public int RecordId { get; set; }
    }
}