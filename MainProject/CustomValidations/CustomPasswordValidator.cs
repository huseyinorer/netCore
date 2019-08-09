using MainProject.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MainProject.CustomValidations
{
    public class CustomPasswordValidator : IPasswordValidator<AppIdentityUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppIdentityUser> manager, AppIdentityUser user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                if (!user.Email.ToLower().Contains(user.UserName.ToLower()))
                    errors.Add(new IdentityError() { Code = "PasswordContainsUserNmae", Description = "Şifre, kullanıcı adınızı içeremez." });
            }
            if (password.ToLower().Contains("1234"))
                errors.Add(new IdentityError() { Code = "PasswordContains1234", Description = "Şifre, 1234 ardışık sayı içeremez" });
            if (password.ToLower().Contains(user.Email.ToLower()))
                errors.Add(new IdentityError() { Code = "PasswordContainsEmail", Description = "Şifre, email adresinizi içeremez" });
            if (errors.Count == 0)
                return Task.FromResult(IdentityResult.Success);

            return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
        }
    }
}