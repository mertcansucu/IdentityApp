using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApp.ViewModels
{
    //veri taşıyacak class
    public class EditViewModel
    {
        // Güncelleme sayfasında girilen bilgierlin zorunluluğunu kaldırdım çünkü zorunlu değil her şeyi güncellemek , birde string.Empity yerine diğer metodla ? ile yapmam lazım 

        public string? Id { get; set; } //güncellemek istediğim userın idsi lazım ben bu idye göre güncelliyicem

        public string? FullName { get; set; } 
        //Username e gerek görmediğim için kaldırdım fullname i alıcam direk
       
        [EmailAddress]
        public string? Email { get; set; } 
        
        
        [DataType(DataType.Password)]
        public string? Password { get; set; } 
        
        
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Parola eşleşmiyor!")]
        public string? ConfirmPassword { get; set; } 

        public IList<string>? SelectedRoles { get; set; }//uyum farkı olmasın diye bu şekilde tanımladım 
    }
}