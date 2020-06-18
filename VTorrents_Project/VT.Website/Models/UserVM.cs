using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VT.Website.Models
{
    public class UserVM
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Username { get; set; }
        [Required]
        [MaxLength(30)]
        public string Password { get; set; }
        [MaxLength(30)]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
        [MaxLength(30)]
        public string Email { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime LastLoggedIn { get; set; }

        public bool isMod { get; set; }

        public bool isAdmin { get; set; }
    }
}
