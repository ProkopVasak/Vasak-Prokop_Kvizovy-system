using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// Pøidání služby pro práci s databází pomocí Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>

    // Použití SQL Serveru jako databázového systému a naètení connection stringu z konfigurace
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Registruje službu CORS v aplikaci
builder.Services.AddCors(options =>
{
    // Vytváøí novou CORS politiku s názvem "MyCors"
    options.AddPolicy("MyCors", builder =>
    {
        builder.AllowAnyOrigin()

            .AllowAnyHeader()   // Povolení jakýchkoliv HTTP hlavièek (napø. Authorization, Content-Type)

            .AllowAnyMethod();  // Povolení všech HTTP metod (GET, POST, PUT, DELETE atd.)
    });
});

// Registruje ASP.NET Core Identity a nastavuje pravidla pro uživatele (IdentityUser)
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options => {

    options.User.RequireUniqueEmail = true; // Každý uživatel musí mít unikátní e-mail

    options.Password.RequiredLength = 8;    // Minimální délka hesla je 8 znakù

    options.Password.RequireDigit = false;  // Heslo nemusí obsahovat èíslo

    options.Password.RequireLowercase = false;  // Heslo nemusí obsahovat malé písmeno

    options.Password.RequireUppercase = false;  // Heslo nemusí obsahovat velké písmeno

    options.Password.RequireNonAlphanumeric = false;    // Heslo nemusí obsahovat speciální znak
})
    .AddRoles<IdentityRole>()   // Pøidání podpory rolí

    .AddEntityFrameworkStores<AppDbContext>();  // Identity bude používat databázi pøes Entity Framework

// Registruje službu autorizace v aplikaci
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireClaim("Admin", "True"));  // Policy vyžaduje claim "Admin" s hodnotou "True"
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())    // Kontrola, zda je aplikace v režimu vývoje
{
    app.UseSwagger();   // Aktivace generování Swagger dokumentace pro API
    app.UseSwaggerUI(); // Aktivace Swagger UI pro interaktivní rozhraní API
}

app.UseHttpsRedirection();

app.UseCors("MyCors");  // Aktivace CORS politiky "MyCors"

app.UseAuthentication();    // Aktivace autentizace (ovìøení identity uživatele)

app.UseAuthorization(); // Aktivace autorizace (kontrola oprávnìní pro pøístup k API)

app.UseFileServer();    // Aktivace serveru pro statické soubory

app.MapFallbackToFile("index.html");  // Pøesmìrování na index.html pro všechny ostatní URL

app.MapControllers();

app.MapGroup("/api").MapCustomIdentityApi<IdentityUser>();  // Mapování vlastní skupiny API s pøizpùsobenou Identity API pro uživatele typu IdentityUser

app.Run();
