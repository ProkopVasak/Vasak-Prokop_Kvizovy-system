using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizSystem.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// P�id�n� slu�by pro pr�ci s datab�z� pomoc� Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    // Pou�it� SQL Serveru jako datab�zov�ho syst�mu a na�ten� connection stringu z konfigurace
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Registrace slu�by CORS v aplikaci
builder.Services.AddCors(options =>
{
    // Vytvo�en� nov� CORS politiky s n�zvem "MyCors"
    options.AddPolicy("MyCors", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:5173")    // Povolen� po�adavk� pouze z t�to adresy
                     .AllowAnyHeader()  // Povolen� jak�chkoliv HTTP hlavi�ek (nap�. Authorization, Content-Type)
                     .AllowAnyMethod()  // Povolen� v�ech HTTP metod (GET, POST, PUT, DELETE atd.)
                     .AllowCredentials();
    });
});

// Registrace Identity a nastaven� pravidel pro u�ivatele (IdentityUser)
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.User.RequireUniqueEmail = true;  // Ka�d� u�ivatel mus� m�t unik�tn� e-mail
    options.Password.RequiredLength = 8;       // Minim�ln� d�lka hesla je 8 znak�
    options.Password.RequireDigit = false;     // Heslo nemus� obsahovat ��slo
    options.Password.RequireLowercase = false; // Heslo nemus� obsahovat mal� p�smeno
    options.Password.RequireUppercase = false; // Heslo nemus� obsahovat velk� p�smeno
    options.Password.RequireNonAlphanumeric = false; // Heslo nemus� obsahovat speci�ln� znak
})
.AddRoles<IdentityRole>()                        // P�id�n� podpory rol�
.AddEntityFrameworkStores<AppDbContext>();      // Identity bude pou��vat datab�zi p�es Entity Framework

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
});



builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.Name = "AuthCookie";
    options.LoginPath = "/api/login";

    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
});



// Registrace slu�by autorizace
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireClaim("Admin", "True"));  // Policy vy�aduje claim "Admin" s hodnotou "True"
});

builder.Services.AddControllers();
// Konfigurace Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())    // Pouze v re�imu v�voje
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyCors");  // Aktivace CORS politiky

app.UseAuthentication();  // Aktivace autentizace (na z�klad� cookies)
app.UseAuthorization();   // Aktivace autorizace (kontrola opr�vn�n�)

app.UseFileServer();

app.MapFallbackToFile("index.html");

app.MapControllers();

// Mapov�n� vlastn� skupiny API s p�izp�sobenou Identity API pro u�ivatele typu IdentityUser
app.MapGroup("/api").MapCustomIdentityApi<IdentityUser>();

app.Run();
