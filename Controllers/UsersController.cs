using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityApp.Models;
using IdentityApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IdentityApp.Controllers
{
    public class UsersController:Controller
    {
        /*
            veritabında değişiklikler yaptığım için bu kodları yazmam lazım
             dotnet ef database drop --force //var olan database sildim
             -daha sonrasında var olan migrationsu sildim ve yeni migrations oluşturdum
             dotnet ef migrations add InitialCreate
             -update olan komutu yazmadın çünkü onun yerine geçen kod ekli 
             dotnet watch run 

             **eğer bir sorun alırsan  bu sırada kodları yine yazıp çalıştır
             dotnet ef database drop --force
             dotnet ef database update
             dotnet watch run

        */
        private UserManager<AppUser> _userManager;//usertablosundaki verileri veritabnından getirdim

        public UsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index(){
            return View(_userManager.Users);//user bilgilerini getirip view içine gönderdim
        }

        public IActionResult Create(){
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model){
            if (ModelState.IsValid)
            {
                var user = new AppUser{UserName = model.Email, Email = model.Email,FullName = model.FullName};//kullanıcının username i olmalı ben bunu direk email yaptım böylece kullanıcı direk emaili username oldu

                IdentityResult result = await _userManager.CreateAsync(user,model.Password);
                //await _userManager.CreateAsync(user); şifresizde kayıt yapabilirim ama kullancı girişi olduğu için uygulamamda böyle yaptım

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("",err.Description);
                }

                /*
                   mrtcnsc@gmail.com - Mert19071. 
                   burak@gmail.com - 123456 username artık direk email oldu öyle ayarladım                
                */
            }
            return View(model);
        }
    }
}