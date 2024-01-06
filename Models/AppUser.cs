using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityApp.Models
{
    public class AppUser: IdentityUser
    {
        public string? FullName { get; set; } //editview de null olarak tanımladığım için ve uymadığı için bunu da o hale getirdim
    }
}