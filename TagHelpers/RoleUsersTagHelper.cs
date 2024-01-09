using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityApp.TagHelpers
{
    //Taghelper kullanmak için _ViewImportcs de aktif etmem lazım

    //<td asp-role-users="@role.Id"></td> td içinde göstereceğim için o bilgilere göre ekledim
    [HtmlTargetElement("td",Attributes = "asp-role-users")]
    public class RoleUsersTagHelper: TagHelper//TagHelper-> View’ın daha okunabilir, anlaşılabilir ve kolay geliştirilebilir hale gelmesine olanak tanır.
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        public RoleUsersTagHelper(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [HtmlAttributeName("asp-role-users")]

        public string RoleId { get; set; } = null!;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var usersNames = new List<string>();
            var role =await _roleManager.FindByIdAsync(RoleId);//role içine _rolemanager den aldığım idye rolleri attım

            if (role != null && role.Name != null)//role ve role.name boş olmadığı durumda gir dedim
            {
                foreach (var user in _userManager.Users)//bütün kullanıcıları gezdim
                {
                    if (await _userManager.IsInRoleAsync(user, role.Name))//o anda eriştiğim user ın role.Name de o role var mı kontrol ediyorum
                    {
                        usersNames.Add(user.UserName ?? "");//varsa bunları ekliyorum ve eğer boş gelme ihtimaline karşı ise "" ile boş bırakıyorum
                    }
                }
                output.Content.SetHtmlContent(usersNames.Count == 0 ? "kullanıcı yok": setHtml(usersNames)); //aldığım bilgileri geri gönderdim,ul içinde liste şeklinde göndermek istesem ->SetHtmlContent veya doğrudan string değer gödermek içinde -> SetContent
                //(usersNames.Count == 0 ? "kullanıcı yok": string.Join(",", usersNames)); burda diyprum ki bulduğum usernames in büyüklüğüne bak 0 sa kullanıcı bulunamadı eğer 0 dan büyükse de aralarına virgül bırakarak gönder
                //Setstring.Join(",", usersNames) bunun yerine ben html listesi şeklinde göndermek istiyorum
                //yani setcontent kullanımı:
                //output.Content.SetContent(usersNames.Count == 0 ? "kullanıcı yok": string.Join(",", usersNames));
                //sethtmlcontent kullanımı:
                //output.Content.SetHtmlContent(usersNames.Count == 0 ? "kullanıcı yok": setHtml(usersNames));
            }
        }

        private string setHtml(List<string> usersNames)
        {
            var html = "<ul>";
            foreach (var item in usersNames)
            {
                html += "<li>" + item +"</li>";
            }

            html += "</ul>";

            return html;
        }
    }
}