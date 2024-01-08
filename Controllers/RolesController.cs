using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers
{
    public class RolesController: Controller
    {
        //detaylı bilgi için rolemanager - user manager

        private readonly RoleManager<AppRole> _roleManager;
        //readonly -> Bu özellik, bir alanın sadece bir kere başlatılmasını ve daha sonra programın çalışma süresi boyunca değiştirilememesini sağlamak için kullanışlıdır. readonly alanlar, genellikle bir sınıfın içinde sabit bir değeri temsil etmek için kullanılır.

        public RolesController(RoleManager<AppRole> roleManager){
            _roleManager = roleManager;
        }

        public IActionResult Index(){
            return View(_roleManager.Roles);//_roleManager.Roles =>IQueryable bir nesnedir yani ben bunu filtreleyip veri tabanına sorgu yazıp istediğim şekilde çekebilirim
        }

        public IActionResult Create(){
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppRole model){

            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(model);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("",err.Description);
                }
            }

            return View(model);
        }

    }
}