using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IdentityApp.Controllers
{
    public class UsersController:Controller
    {
        private UserManager<IdentityUser> _userManager;//usertablosundaki verileri veritabnından getirdim

        public UsersController(UserManager<IdentityUser> userManager)
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
                var user = new IdentityUser{UserName = model.UserName, Email = model.Email};

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
                   burak@gmail.com - 123456 - burakotan                
                */
            }
            return View(model);
        }
    }
}