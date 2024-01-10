using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<IdentityContext>(
    options => options.UseSqlite(builder.Configuration["ConnectionStrings:SqLite_Connection"]));//vuraya birden fazla database ekleyebilirim

builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();//IdentityContext.cs de database işlemlerini yapacağımdan onu yazdım
//IdentityUser => yerine ben artık AppUser la çalışıcam çünkü ekstra kullanıcı bilgilerini almak için oluşturdum
//IdentityRole ektradan rolleride oluşturduğum clastan artık alacağım için değitirdim

//configure asp.net core identity ile kuralları değiştirebilirim,mresela parola kurallarını ve mail adresini bir kere kullanma, birde istersem username girilen karakterleri seçerek türkçe karakteri kapatabilirim

//**AddDefaultTokenProviders() bu bize->parola yenileme,sıfırlama,email sıfırlama email onaylı hale getirmek için bize token bilgisi üretir

builder.Services.Configure<IdentityOptions>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    
    options.User.RequireUniqueEmail = true;

    options.User.AllowedUserNameCharacters = "abcdefghiıjklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

    
    // options.User.AllowedUserNameCharacters = "abcdefgijklmnopqrstuvwxyz"; bunu yorum satırı yaptım çünkü ben username için mail adresini otomatik yaptım ve mail adresin de @ ve . olduğu için hata verdi buna engel olmak için, bunun yerine önceki haliyle username ayrı bir şekilde alabilirim ama buna şimdilik gerek olmadığı için yapmadım

    //kullanıcı login işleminde ben kullanıcıya 5 hak vericem eğer 5 seferde login olamazsa hesabını 5 dk kitliyicem ve kullanıcı giriş yapamıyacak
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;


    //giriş için email onayı olmasını istediğim için bunu aktif etmem lazım başlangıçta bu false du nem bunu aktif yaptım
    options.SignIn.RequireConfirmedEmail = true;
});

builder.Services.ConfigureApplicationCookie(options =>{//cookie ayarları
    options.LoginPath = "/Account/Login";//default halide bu login girişi için buraya yönlendirir beni
    options.AccessDeniedPath = "/Account/AccessDenied";//giriş yapan kullanıcı rolü yani yetkisi yetersizse sayfalara erişmesini engelliyorum ve kullanıcıya yetkisiz kişi olduğunu söylüyorum
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);//ben bu yazdığım iki kodla diyorum ki kullanıcı 30 gün içerisinde giriş yapsın veya yapmasın cookisini sakla sonra sil, ama diyelim 15. gün ben giriş yaptım süre 30 gün olarak yeniden başlar sonra kayıt silinir ve kullanıcı yeniden giriş yaparak giriş yapmalıdır
    

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

//kimlik doğrulama işlemleri
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

IdentitySeedData.IdentityTestUser(app);//IdentitySeedData yani test verilerini ekledim

app.Run();
