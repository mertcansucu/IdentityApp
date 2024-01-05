using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Models
{
    public class IdentityContext: IdentityDbContext<IdentityUser>
    {
        /* bu yöntemlede bağlantı sağlanabilir ama ben connections stringi dışardan vermek istediğim için böyle yapmadım
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MyDb;Trusted_Connection=True;");
        }
        */

        public IdentityContext(DbContextOptions<IdentityContext> options):base(options){
            
        }
    }
}