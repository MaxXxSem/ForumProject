using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ForumProject.Models.ViewModels
{
    public class UserAddsRecordViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string MainPhoto { get; set; }
    }
}