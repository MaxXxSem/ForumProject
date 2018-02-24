using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ForumProject.Models.Data;
using System.ComponentModel.DataAnnotations;

namespace ForumProject.Models.ViewModels
{
    public class ProfileViewModel
    {
        public ProfileViewModel()
        {
            Records = new HashSet<Records>();
            LikedRecords = new HashSet<Records>();
            Subscriptions = new HashSet<Users>();
            Subscribers = new HashSet<Users>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string MainPhoto { get; set; }

        public virtual ICollection<Records> Records { get; set; }

        public virtual ICollection<Records> LikedRecords { get; set; }

        public virtual ICollection<Users> Subscriptions { get; set; }

        public virtual ICollection<Users> Subscribers { get; set; }
    }
}