using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Models
{
    public class IdentityContext: IdentityDbContext<AppUser, AppRole, string>
    {
        /* bu yöntemlede bağlantı sağlanabilir ama ben connections stringi dışardan vermek istediğim için böyle yapmadım
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MyDb;Trusted_Connection=True;");

            IdentityUser =>AppUser olarak değiştirdim çünkü o class içinden çağırıyorum zaten

            AppRole, string böyle yapmamın nedeni Approle çağırdığım için keyi de belirtmem lazım yani id sini int de yapabilirim ama string olarak default kalsın dedim
        }
        */

        public IdentityContext(DbContextOptions<IdentityContext> options):base(options){
            
        }
    }
}