using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// P�id�n� slu�by pro pr�ci s datab�z� pomoc� Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>

    // Pou�it� SQL Serveru jako datab�zov�ho syst�mu a na�ten� connection stringu z konfigurace
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Registruje slu�bu CORS v aplikaci
builder.Services.AddCors(options =>
{
    // Vytv��� novou CORS politiku s n�zvem "MyCors"
    options.AddPolicy("MyCors", builder =>
    {
        builder.AllowAnyOrigin()

            .AllowAnyHeader()   // Povolen� jak�chkoliv HTTP hlavi�ek (nap�. Authorization, Content-Type)

            .AllowAnyMethod();  // Povolen� v�ech HTTP metod (GET, POST, PUT, DELETE atd.)
    });
});

// Registruje ASP.NET Core Identity a nastavuje pravidla pro u�ivatele (IdentityUser)
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options => {

    options.User.RequireUniqueEmail = true; // Ka�d� u�ivatel mus� m�t unik�tn� e-mail

    options.Password.RequiredLength = 8;    // Minim�ln� d�lka hesla je 8 znak�

    options.Password.RequireDigit = false;  // Heslo nemus� obsahovat ��slo

    options.Password.RequireLowercase = false;  // Heslo nemus� obsahovat mal� p�smeno

    options.Password.RequireUppercase = false;  // Heslo nemus� obsahovat velk� p�smeno

    options.Password.RequireNonAlphanumeric = false;    // Heslo nemus� obsahovat speci�ln� znak
})
    .AddRoles<IdentityRole>()   // P�id�n� podpory rol�

    .AddEntityFrameworkStores<AppDbContext>();  // Identity bude pou��vat datab�zi p�es Entity Framework

// Registruje slu�bu autorizace v aplikaci
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireClaim("Admin", "True"));  // Policy vy�aduje claim "Admin" s hodnotou "True"
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())    // Kontrola, zda je aplikace v re�imu v�voje
{
    app.UseSwagger();   // Aktivace generov�n� Swagger dokumentace pro API
    app.UseSwaggerUI(); // Aktivace Swagger UI pro interaktivn� rozhran� API
}

app.UseHttpsRedirection();

app.UseCors("MyCors");  // Aktivace CORS politiky "MyCors"

app.UseAuthentication();    // Aktivace autentizace (ov��en� identity u�ivatele)

app.UseAuthorization(); // Aktivace autorizace (kontrola opr�vn�n� pro p��stup k API)

app.UseFileServer();    // Aktivace serveru pro statick� soubory

app.MapFallbackToFile("index.html");  // P�esm�rov�n� na index.html pro v�echny ostatn� URL

app.MapControllers();

app.MapGroup("/api").MapCustomIdentityApi<IdentityUser>();  // Mapov�n� vlastn� skupiny API s p�izp�sobenou Identity API pro u�ivatele typu IdentityUser

app.Run();
