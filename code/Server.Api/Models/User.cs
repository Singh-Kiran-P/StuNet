using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Server.Api.Models
{
    public class User : IdentityUser
    {
        // public int id { get; set; }
        // public string email { get; set; }
        // public string password { get; set; }

        // public string salt { get; set; }
        public ICollection<Answer> answers;
        
    }
}