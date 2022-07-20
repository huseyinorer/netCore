using Microsoft.AspNetCore.Identity;
using System;

namespace MainProject.Identity
{
    public class AppIdentityUser : IdentityUser
    {
        public string City { get; set; }
        public DateTime BirthDate { get; set; }
        public int Gender { get; set; }

    }
}
