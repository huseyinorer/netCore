using Microsoft.AspNetCore.Identity;

namespace core2.Identity
{
    public class AppIdentityUser : IdentityUser
    {
        public int Age { get; set; }

    }
}
