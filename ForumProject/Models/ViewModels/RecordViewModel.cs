using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ForumProject.Models.Data;
using System.ComponentModel.DataAnnotations;

namespace ForumProject.Models.ViewModels
{
    public class RecordViewModel
    {
        public RecordViewModel()
        {
            Comments = new HashSet<Comments>();
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [Required]
        [StringLength(1024)]
        public string Text { get; set; }

        public DateTime Date { get; set; }

        public virtual ICollection<Comments> Comments { get; set; }

        public virtual Users User { get; set; }
    }
}