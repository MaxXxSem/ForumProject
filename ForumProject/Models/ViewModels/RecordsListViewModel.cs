using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ForumProject.Models.Data;

namespace ForumProject.Models.ViewModels
{
    public class RecordsListViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [Required]
        [StringLength(1024)]
        public string Text { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public int SubtopicId { get; set; }

        public virtual Users User { get; set; }

        public virtual ICollection<Users> UsersWhoLike { get; set; }
    }
}