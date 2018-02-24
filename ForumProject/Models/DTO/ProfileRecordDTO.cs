using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ForumProject.Models.DTO
{
    public class ProfileRecordDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [Required]
        [StringLength(1024)]
        public string Text { get; set; }
        
        public DateTime Date { get; set; }

        public int UsersWhoLikeCount { get; set; }
    }
}