using MainProject.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using System.Threading.Tasks;

namespace MainProject.TagHelpers
{
    [HtmlTargetElement("td", Attributes = "user-roles")]
    public class UserRolesName : TagHelper
    {
        public UserManager<AppIdentityUser> UserManager { get; set; }

        public UserRolesName(UserManager<AppIdentityUser> userManager)
        {
            this.UserManager = userManager;
        }

        [HtmlAttributeName("user-roles")]
        public string UserId { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = await UserManager.FindByIdAsync(UserId);
            var roles = await UserManager.GetRolesAsync(user);

            string html = string.Empty;

            roles.ToList().ForEach(x => { html += $"<span class='badge badge-primary'>{x}</span>"; });
            output.Content.SetHtmlContent(html);


        }
    }
}
