using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Models
{
    public class IdentitySeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Admin_123";

        public static async void IdentityTestUser(IApplicationBuilder app){
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IdentityContext>();

            if (context.Database.GetAppliedMigrations().Any())//eğer hazır bir veitabanı varsa direkt çalıştır dedim, yani migrations çalıştırırken yazdığım:dotnet ef database update --context IdentityContext koduna karşılık geliyor
            {
                context.Database.Migrate();
            }

            var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();//user veritabanını çağırdım

            var user = await userManager.FindByNameAsync(adminUser);//user içine adminUser a bak kayıtlı biri var mı diye eğer yoksa da yeni kayıt oluşturdum,veritabanında admin adında bir kullanıcı yoksa

            if (user == null)
            {
                user = new IdentityUser {
                    UserName = adminUser,
                    Email = "mrtcnscc@gmail.com",
                    PhoneNumber = "5454414690"
                };

                await userManager.CreateAsync(user,adminPassword);//adminin password bilgisini veritabanında tutmuyorum çünkü bilgiler sızdırılırsa veriabına girip admin password görülmesin diye
            }

        }
    }
}