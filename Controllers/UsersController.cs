using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
    }
}