using System;
using System.Collections.Generic;
using System.Text;

namespace VT.Models.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime LastLoggedIn { get; set; }

        public bool isMod { get; set; }

        public bool isAdmin { get; set; }
    }
}
