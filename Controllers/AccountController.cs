using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityApp.Models;
using IdentityApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers
{
    public class AccountController: Controller
    {
        private UserManager<AppUser> _userManager;//usertablosundaki verileri veritabnından getirdim

        private RoleManager<AppRole> _roleManager;//roletablosundaki verileri veritabnından getirdim

        private SignInManager<AppUser> _signInManager;

        public AccountController(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;

            _roleManager = roleManager;

            _signInManager = signInManager;//login işlemi olunca kullanıcıya cookie bilgisi göndermek için ekledim
        }
        public IActionResult Login(){
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model){

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    await _signInManager.SignOutAsync();//başlangıçta kullanıcı daha önce giriş yapmışsa cookie sini sıfırladım

                    //yeni cookie oluşturdum
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe,true);
                    /*
                    bu kodda:var result = await _signInManager.PasswordSignInAsync(user, model.Password, 
                    model.RememberMe,=> burda eğer bu şeçili olursa tarayıcı kapandığında bile tekrar uygulamayı açınca kullanıcı girişi olmuş şekilde gelir beni hatırla yani
                    true)=> bu kod ise programcs de ben 5 hak vermiştim kullanıcıya hatalı giriş için ben bunu geçerli kıldım bunu false yaparsam kullanıcı istediği kadar hatalı girebilir
                    */

                    if (result.Succeeded)//giriş başarılı durumu
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);//ben burda eğer kullanıcı giriş yaptıysa başta dediğim 5 kere hatalı girme bilgisine getirdim yani 3 kere yanlış girdi ve giriş yaptıysa tekrar giriş yaptığında 5 kere hatalı girme hakkı verdim
                        await _userManager.SetLockoutEndDateAsync(user,null);//bekleme süresini de sıfırladım


                        return RedirectToAction("Index","Home");
                    }else if (result.IsLockedOut)//kullanıcı girişi başarısızsa
                    {
                        var LockoutDate = await _userManager.GetLockoutEndDateAsync(user);
                        var timeleft = LockoutDate.Value - DateTime.UtcNow;//ben burdaki kodla şunu aldım eğer kullanıcı 5 kere hatalı girerse 5 dakida bekle demiştim ya kullanıcıya bu süreyi göstermek için aldım burda kaç dakikası kaldığını göstericem ve ona söyliyicem

                        ModelState.AddModelError("",$"Hesabınız kilitlendi, lütfen {timeleft.Minutes} dakika sonra tekrar deneyiniz!");
                    }else
                    {
                        ModelState.AddModelError("","Parolanız hatalı!");
                    }

                    
                }else{
                    ModelState.AddModelError("","Bu Email adresiyle bir hesap bulunamadı!");
                }
            }


            return View(model);
        }
    }
}