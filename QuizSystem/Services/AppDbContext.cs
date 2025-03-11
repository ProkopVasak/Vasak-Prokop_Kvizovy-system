using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizSystem.Models;

namespace QuizSystem.Services
{

    // AppDbContext dědí z IdentityDbContext pro práci s Identity (autentizace a autorizace uživatelů)
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {

        // Konstruktor pro inicializaci kontextu s přenesením možností konfigurovat DbContext
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSety pro práci s entitami Quiz, Question, Answer v databázi
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        // Tato metoda se volá při vytváření modelu
        protected override void OnModelCreating(ModelBuilder modelBuilder)  
        {

            // Zavolání základní metody pro inicializaci identity
            base.OnModelCreating(modelBuilder);

            // Definice vztahů mezi entitami

            // Kvíz má vztah 1:N s otázkami (quiz -> question)
            modelBuilder.Entity<Quiz>()
                 .HasOne<IdentityUser>()
                 .WithMany()
                 .HasForeignKey(q => q.AuthorId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Restrict);

            // Otázka má vztah 1:N s odpověďmi (question -> answer)
            modelBuilder.Entity<Question>()
                .HasOne<Quiz>()
                .WithMany(q => q.Questions)
                .HasForeignKey(q => q.QuizId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Odpověď má vztah 1:N s otázkami (answer -> question)
            modelBuilder.Entity<Answer>()
                .HasOne<Question>()
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Seedování dat: vytvoření rolí pro admina a uživatele
            var adminRoleId = Guid.NewGuid().ToString();
            var userRoleId = Guid.NewGuid().ToString();

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = userRoleId, Name = "User", NormalizedName = "USER" }
            );

            // Vytvoření admin uživatele s testovacími daty
            var adminUserId = Guid.NewGuid().ToString();

            modelBuilder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = adminUserId,
                    UserName = "admin@admin.cz",
                    NormalizedUserName = "ADMIN@ADMIN.CZ",
                    Email = "admin@admin.cz",
                    NormalizedEmail = "ADMIN@ADMIN.CZ",
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "admin"),  // Nastavení hashe hesla
                    SecurityStamp = "razitko",  // Bezpečnostní razítko
                    EmailConfirmed = true,  // Potvrzený email

                }
            );

            // Přiřazení admina k roli "Admin"
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = adminUserId, RoleId = adminRoleId }
            );

            // Přiřazení claimu "Admin" uživateli
            modelBuilder.Entity<IdentityUserClaim<string>>().HasData(
                new IdentityUserClaim<string>
                {
                    Id = 1,
                    UserId = adminUserId,
                    ClaimType = "Admin",
                    ClaimValue = "True"
                }
            );

            // Vytvoření GUID pro kvízy, otázky a odpovědi
            Guid[] quizIds = new Guid[30];
            Guid[] questionIds = new Guid[60];
            Guid[] answerIds = new Guid[240];

            for (int i = 0; i < 30; i++)
            {
                quizIds[i] = Guid.NewGuid();
            }

            for (int i = 0; i < 60; i++)
            {
                questionIds[i] = Guid.NewGuid();
            }

            for (int i = 0; i < 240; i++)
            {
                answerIds[i] = Guid.NewGuid();
            }

            // Seedování dat pro kvízy

            modelBuilder.Entity<Quiz>().HasData(
                quizIds.Select((id, index) => new Quiz
                {
                    Id = id,
                    Name = $"Testovací kvíz {index + 1}",
                    Description = $"Popis pro testovací kvíz {index + 1}",
                    AuthorId = adminUserId 
                }).ToArray()
            );

            modelBuilder.Entity<Question>().HasData(
                questionIds.Select((id, index) => new Question
                {
                    Id = id,
                    Text = index % 2 == 0 ? "Kolik je 5 + 5?" : "Kolik je 10 / 2?",
                    QuizId = quizIds[index / 2]
                }).ToArray()
            );

            modelBuilder.Entity<Answer>().HasData(
                answerIds.Select((id, index) => new Answer
                {
                    Id = id,
                    Text = index % 4 == 0
                        ? (index / 4 % 2 == 0 ? "10" : "5") 
                        : (index % 4 == 1 ? "15" : (index % 4 == 2 ? "20" : "25")),
                    IsCorrect = index % 4 == 0 && (index / 4 % 2 == 0 || index / 4 % 2 == 1 && index % 4 == 0),
                    QuestionId = questionIds[index / 4]
                }).ToArray()
            );


        }
    }
}