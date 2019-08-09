using MainProject.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainProject.CustomValidations
{
    public class CustomUserValidator : IUserValidator<AppIdentityUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppIdentityUser> manager, AppIdentityUser user)
        {
            List<IdentityError> errors = new List<IdentityError>();
            string[] Digits = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", };
            foreach (var item in Digits)
            {
                if (user.UserName[0].ToString() == item)
                    errors.Add(new IdentityError { Code = "UserNameContainsFirstLetterDigitsContain", Description = "Kullanıcı adı sayısal karakter ile başlayamaz." });

            }

            if (errors.Count == 0)
                return Task.FromResult(IdentityResult.Success);

            return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
        }
    }
}
