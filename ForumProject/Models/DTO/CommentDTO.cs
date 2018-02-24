using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ForumProject.Models.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [StringLength(128)]
        public string Text { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(64)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        public string UserMainPhoto { get; set; }

        public int UsersWhoLikeCount { get; set; }
    }
}