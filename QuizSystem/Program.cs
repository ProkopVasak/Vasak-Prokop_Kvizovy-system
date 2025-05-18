using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizSystem.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Pøidání služby pro práci s databází pomocí Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    // Použití SQL Serveru jako databázového systému a naètení connection stringu z konfigurace
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Registrace služby CORS v aplikaci
builder.Services.AddCors(options =>
{
    // Vytvoøení nové CORS politiky s názvem "MyCors"
    options.AddPolicy("MyCors", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:5173")    // Povolení požadavkù pouze z této adresy
                     .AllowAnyHeader()  // Povolení jakýchkoliv HTTP hlavièek (napø. Authorization, Content-Type)
                     .AllowAnyMethod()  // Povolení všech HTTP metod (GET, POST, PUT, DELETE atd.)
                     .AllowCredentials();
    });
});

// Registrace Identity a nastavení pravidel pro uživatele (IdentityUser)
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.User.RequireUniqueEmail = true;  // Každý uživatel musí mít unikátní e-mail
    options.Password.RequiredLength = 8;       // Minimální délka hesla je 8 znakù
    options.Password.RequireDigit = false;     // Heslo nemusí obsahovat èíslo
    options.Password.RequireLowercase = false; // Heslo nemusí obsahovat malé písmeno
    options.Password.RequireUppercase = false; // Heslo nemusí obsahovat velké písmeno
    options.Password.RequireNonAlphanumeric = false; // Heslo nemusí obsahovat speciální znak
})
.AddRoles<IdentityRole>()                        // Pøidání podpory rolí
.AddEntityFrameworkStores<AppDbContext>();      // Identity bude používat databázi pøes Entity Framework

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



// Registrace služby autorizace
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireClaim("Admin", "True"));  // Policy vyžaduje claim "Admin" s hodnotou "True"
});

builder.Services.AddControllers();
// Konfigurace Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())    // Pouze v režimu vývoje
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyCors");  // Aktivace CORS politiky

app.UseAuthentication();  // Aktivace autentizace (na základì cookies)
app.UseAuthorization();   // Aktivace autorizace (kontrola oprávnìní)

app.UseFileServer();

app.MapFallbackToFile("index.html");

app.MapControllers();

// Mapování vlastní skupiny API s pøizpùsobenou Identity API pro uživatele typu IdentityUser
app.MapGroup("/api").MapCustomIdentityApi<IdentityUser>();

app.Run();
