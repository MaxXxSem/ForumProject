using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ForumProject.Models.Identity
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Fill the field")]
        [StringLength(64, ErrorMessage = "Length must be less than 64")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Fill the field")]
        [StringLength(64, ErrorMessage = "Length must be less than 64")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Fill the field")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Fill the field")]
        [Compare("Password", ErrorMessage = "Must be the same with password field")]
        public string ConfirmPassword { get; set; }
    }
}