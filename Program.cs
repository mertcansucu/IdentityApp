using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<IdentityContext>(
    options => options.UseSqlite(builder.Configuration["ConnectionStrings:SqLite_Connection"]));//vuraya birden fazla database ekleyebilirim

builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<IdentityContext>();//IdentityContext.cs de database işlemlerini yapacağımdan onu yazdım
//IdentityUser => yerine ben artık AppUser la çalışıcam çünkü ekstra kullanıcı bilgilerini almak için oluşturdum
//IdentityRole ektradan rolleride oluşturduğum clastan artık alacağım için değitirdim

//configure asp.net core identity ile kuralları değiştirebilirim,mresela parola kurallarını ve mail adresini bir kere kullanma, birde istersem username girilen karakterleri seçerek türkçe karakteri kapatabilirim

builder.Services.Configure<IdentityOptions>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    
    options.User.RequireUniqueEmail = true;

    options.User.AllowedUserNameCharacters = "abcdefghiıjklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

    
    // options.User.AllowedUserNameCharacters = "abcdefgijklmnopqrstuvwxyz"; bunu yorum satırı yaptım çünkü ben username için mail adresini otomatik yaptım ve mail adresin de @ ve . olduğu için hata verdi buna engel olmak için, bunun yerine önceki haliyle username ayrı bir şekilde alabilirim ama buna şimdilik gerek olmadığı için yapmadım
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

IdentitySeedData.IdentityTestUser(app);//IdentitySeedData yani test verilerini ekledim

app.Run();
