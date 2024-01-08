using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityApp.Models;
using IdentityApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        private RoleManager<AppRole> _roleManager;//roletablosundaki verileri veritabnından getirdim

        public UsersController(UserManager<AppUser> userManager,RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;

            _roleManager = roleManager;
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

        public async Task<IActionResult> Edit(string id){
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var user = await _userManager.FindByIdAsync(id);//gönderdiğim id user ın idsi ile eşleşen user ı aldım

            if (user != null)
            {
                ViewBag.Roles = await _roleManager.Roles.Select(i => i.Name).ToListAsync();//viewbag üzerinden ben role tablosundan sadece role isimlerini aldım ve kullanıcı seçsin diye view sayfasında burdan aldığım bilgileri getirip seçtiricem

                return View(new EditViewModel{
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    SelectedRoles = await _userManager.GetRolesAsync(user)//bu komutla kullanıcıya atanmış rolleri ben alabilicem veritabanından ve sayfada göstericem
                }); 
            }
            
            // eğer kullanıcı yoksa index sayfasına göndeerdim
            return RedirectToAction("Index");
        }

        [HttpPost]//kullanıcı güncellemesi 
        public async Task<IActionResult> Edit(string id, EditViewModel model){
            if (id != model.Id)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);//Id ile o kullanıcıyı aldım

                if (user != null)
                {
                    user.FullName = model.FullName;//kullanıcı tablosundaki bilgiyi modeldeki değerle güncelledim
                    user.Email = model.Email;//kullanıcı tablosundaki bilgiyi modeldeki değerle güncelledim

                    var result = await _userManager.UpdateAsync(user);

                    //şifre güncellemesini zorunlu yapmadım ama admin istediği zaman kullanıcı şifresini değiştirebilir onuda:
                    if (result.Succeeded && !string.IsNullOrEmpty(model.Password))//result.Succeeded && !string.IsNullOrEmpty(model.Password) burda diyorum ki durum başarılı ve model view de password alanı boş değilse bu koşulun içine gir ve kullanıcı da kayıtlı olan şifreyi sil ve model view de yazdığım şifreyi userın yeni şifresi yap
                    {
                        await _userManager.RemovePasswordAsync(user);
                        await _userManager.AddPasswordAsync(user, model.Password);
                    }
                    
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    foreach (IdentityError err in result.Errors)
                    {
                        ModelState.AddModelError("",err.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id){
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            
            return RedirectToAction("Index");
        }

    }
}