using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ForumProject.Models.DTO
{
    public class SubscribtionsDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string MainPhoto { get; set; }

        [Required]
        [StringLength(16)]
        public string LevelInfoName { get; set; }
    }
}