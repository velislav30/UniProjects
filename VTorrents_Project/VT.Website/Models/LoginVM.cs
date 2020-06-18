using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VT.Website.Models
{
    public class LoginVM
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
