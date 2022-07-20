using MainProject.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MainProject.ClaimProvider
{
    public class ClaimProvider : IClaimsTransformation
    {
        public UserManager<AppIdentityUser> _userManager { get; set; }

        public ClaimProvider(UserManager<AppIdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal != null && principal.Identity.IsAuthenticated)
            {
                var identity = principal.Identity as ClaimsIdentity;
                var user = await _userManager.FindByNameAsync(identity.Name);
                if (user != null)
                {
                    if (user.City != null)
                    {
                        if (!principal.HasClaim(c => c.Type == "City"))
                        {
                            var cityClaim = new Claim("city", user.City, ClaimValueTypes.String, "Internal");
                            identity.AddClaim(cityClaim);
                        }
                    }

                    if (user.BirthDate != null)
                    {
                        var today = DateTime.Today;
                        var age = today.Year - user.BirthDate.Year;

                        if (age > 15)
                        {
                            var yearClaim = new Claim("year", true.ToString(), ClaimValueTypes.String, "Internal");
                            identity.AddClaim(yearClaim);
                        }

                    }

                }


            }
            return principal;
        }
    }
}