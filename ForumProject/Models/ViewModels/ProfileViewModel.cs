using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ForumProject.Models.Data;
using ForumProject.Models.DTO;
using System.ComponentModel.DataAnnotations;

namespace ForumProject.Models.ViewModels
{
    public class ProfileViewModel
    {
        public ProfileViewModel()
        {
            Records = new HashSet<ProfileRecordDTO>();
            LikedRecords = new HashSet<ProfileRecordDTO>();
            Subscriptions = new HashSet<SubscribtionsDTO>();
            Subscribers = new HashSet<SubscribtionsDTO>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string MainPhoto { get; set; }

        public virtual ICollection<ProfileRecordDTO> Records { get; set; }

        public virtual ICollection<ProfileRecordDTO> LikedRecords { get; set; }

        public virtual ICollection<SubscribtionsDTO> Subscriptions { get; set; }

        public virtual ICollection<SubscribtionsDTO> Subscribers { get; set; }
    }
}