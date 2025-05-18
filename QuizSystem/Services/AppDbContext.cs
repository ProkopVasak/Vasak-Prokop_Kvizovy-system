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

            int quizCount = 36;
            int questionsPerQuiz = 1;
            int questionCount = quizCount * questionsPerQuiz; // 36
            int answersPerQuestion = 4;
            int answerCount = questionCount * answersPerQuestion; // 144

            // Vygenerujeme GUID pro kvízy, otázky a odpovědi:
            Guid[] quizIds = new Guid[quizCount];
            for (int i = 0; i < quizCount; i++)
            {
                quizIds[i] = Guid.NewGuid();
            }

            Guid[] questionIds = new Guid[questionCount];
            for (int i = 0; i < questionCount; i++)
            {
                questionIds[i] = Guid.NewGuid();
            }

            Guid[] answerIds = new Guid[answerCount];
            for (int i = 0; i < answerCount; i++)
            {
                answerIds[i] = Guid.NewGuid();
            }

            // Název a popisy kvízů
            string[] quizTitles =
            {
                "Základy matematiky", "Historie středověku", "Přírodní vědy", "Literatura 19. století",
                "Geografie Evropy", "Sportovní fakta", "Hudební legendy", "Filmové klasiky",
                "Základy informatiky", "Moderní technologie", "Kultura a umění", "Přírodní divy světa",
                "Astronomie", "Biologie člověka", "Ekonomie 101", "Politika a společnost",
                "Filozofie", "Sportovní strategie", "Přírodní katastrofy", "Novinky ve vědě",
                "Mythologie světa", "Zdraví a výživa", "Vzdělávací trendy", "Automobilový průmysl",
                "Televize a média", "Psychologie", "Ekologie", "Cestování", "Umělecká díla",
                "Architektura", "Kulinářské speciality", "Historické události", "Obchod a management",
                "Cizí jazyky", "Sportovní rekordy", "Knižní literatura"
            };

            string[] quizDescriptions =
            {
                "Test základních matematických znalostí.",
                "Otázky týkající se středověké historie.",
                "Otázky z fyziky, chemie a biologie.",
                "Známá díla a autoři 19. století.",
                "Znalosti o hlavních městech a zemích Evropy.",
                "Test o významných sportovních událostech a osobnostech.",
                "Otázky o slavných hudebních osobnostech a žánrech.",
                "Ikonické filmy všech dob.",
                "Test o počítačové vědě a programování.",
                "Otázky o nejnovějších technologiích a inovacích.",
                "Přehled základních směrů v umění a kultuře.",
                "Prozkoumejte nejznámější přírodní památky.",
                "Kosmické jevy a vesmírné objekty.",
                "Základy lidské anatomie a fyziologie.",
                "Základy ekonomie a finančních trhů.",
                "Otázky o politických systémech a historických událostech.",
                "Významní filozofové a jejich myšlenky.",
                "Otázka ze sportovních pravidel.",
                "Otázka o přírodních jevech.",
                "Test ze základů kvantové mechaniky.",
                "Otázka z řecké mytologie.",
                "Test o doporučeném denním příjmu ovoce a zeleniny.",
                "Otázka na vysvětlení MOOC.",
                "Test z automobilového světa.",
                "Otázka na televizní seriály.",
                "Test z psychologie a behaviorismu.",
                "Otázka o obnovitelných zdrojích energie.",
                "Test z evropské geografie.",
                "Otázka o slavném uměleckém díle.",
                "Test z moderní architektury.",
                "Otázka o původu sushi.",
                "Historická otázka o druhé světové válce.",
                "Test obchodních metod – SWOT analýza.",
                "Otázka na mezinárodní obchodní jazyk.",
                "Test ze sportovních rekordů.",
                "Otázka z české literatury."
            };

            // Otázky – každá otázka má vlastní text
            string[] questionTexts =
            {
                "Kolik je 10 děleno 5?", // Základy matematiky
                "Kdo byl významným panovníkem ve středověku?", // Historie středověku
                "Co studuje biologie?", // Přírodní vědy
                "Který autor patří do literatury 19. století?", // Literatura 19. století
                "Které město je hlavním městem Francie?", // Geografie Evropy
                "Kolik hráčů je v základním rozestavení ve fotbalovém týmu?", // Sportovní fakta
                "Která kapela se proslavila albem \"Abbey Road\"?", // Hudební legendy
                "Který film režíroval Francis Ford Coppola?", // Filmové klasiky
                "Co je binární kód?", // Základy informatiky
                "Co je to AI?", // Moderní technologie
                "Který malíř je autorem 'Mona Lisy'?", // Kultura a umění
                "Který z těchto je považován za přírodní div?", // Přírodní divy světa
                "Jak se jmenuje nejbližší hvězda k Zemi?", // Astronomie
                "Kolik kostí má dospělý lidský skelet?", // Biologie člověka
                "Co je inflace?", // Ekonomie 101
                "Který z těchto výrazů charakterizuje pluralitu?", // Politika a společnost
                "Který filozof je autorem díla 'Kritika čistého rozumu'?", // Filozofie
                "Co je to offsides ve fotbale?", // Sportovní strategie
                "Co je tsunami?", // Přírodní katastrofy
                "Co studuje kvantová mechanika?", // Novinky ve vědě
                "Které božstvo je spojené s hromem v řecké mytologii?", // Mythologie světa
                "Kolik denně doporučuje Světová zdravotnická organizace ovoce a zeleniny?", // Zdraví a výživa
                "Co znamená MOOC?", // Vzdělávací trendy
                "Která značka je známá modelem Mustang?", // Automobilový průmysl
                "Který televizní seriál získal cenu Emmy za nejlepší drama?", // Televize a média
                "Kdo je zakladatelem behaviorismu?", // Psychologie
                "Co je obnovitelná energie?", // Ekologie
                "Jaké je hlavní město Itálie?", // Cestování
                "Které umělecké dílo vytvořil Michelangelo?", // Umělecká díla
                "Která stavba je symbolem moderní architektury v Dubaji?", // Architektura
                "Z jaké země pochází sushi?", // Kulinářské speciality
                "Kdy začala druhá světová válka?", // Historické události
                "Co je to SWOT analýza?", // Obchod a management
                "Jaký jazyk se považuje za univerzální v mezinárodním obchodu?", // Cizí jazyky
                "Kdo drží rekord v maratonu?", // Sportovní rekordy
                "Která kniha je považována za klasiku české literatury?" // Knižní literatura
            };

            // Pro každou otázku definujeme vlastní sadu 4 odpovědí
            string[][] answerOptions = new string[][]
            {
                new string[] { "1.1", "4", "2", "10" },
                new string[] { "Karel IV.", "Jan Hus", "Albrecht z Kolína", "Přemysl Otakar II." },
                new string[] { "Lidské tělo", "Planety", "Matematiku", "Počítače" },
                new string[] { "Jan Neruda", "Karel Hynek Mácha", "Petr Bezruč", "Bohumil Hrabal" },
                new string[] { "Lyon", "Marseille", "Paříž", "Bordeaux" },
                new string[] { "9", "10", "11", "12" },
                new string[] { "The Beatles", "Queen", "Led Zeppelin", "Rolling Stones" },
                new string[] { "Kmotr", "Casablanca", "Schindlerův seznam", "Matrix" },
                new string[] { "Jazyk počítače", "Programovací jazyk", "Datový formát", "Kód pro tajné zprávy" },
                new string[] { "Artificial Intelligence", "Analytical Instrument", "Automotive Innovation", "Advanced Internet" },
                new string[] { "Vincent van Gogh", "Pablo Picasso", "Leonardo da Vinci", "Rembrandt" },
                new string[] { "Grand Canyon", "Eiffelova věž", "Koloseum", "Socha Svobody" },
                new string[] { "Proxima Centauri", "Sirius", "Betelgeuse", "Polaris" },
                new string[] { "206", "205", "210", "201" },
                new string[] { "Růst cen", "Pokles cen", "Stabilní ceny", "Neexistence peněz" },
                new string[] { "Jednolitost", "Rozmanitost", "Monarchie", "Totalitu" },
                new string[] { "Immanuel Kant", "Sokrates", "Aristoteles", "Nietzsche" },
                new string[] { "Pravidlo ofsajdu", "Volná hra", "Penalty", "Hlavní útočník" },
                new string[] { "Obrovská vlna", "Malenká vlna", "Vichřice", "Sucho" },
                new string[] { "Mikročástice", "Makro struktury", "Galaxie", "Organismus" },
                new string[] { "Héfaistos", "Zeus", "Poseidón", "Hermes" },
                new string[] { "5 porcí", "3 porce", "8 porcí", "2 porce" },
                new string[] { "Massive Open Online Course", "Modern Online Operating Course", "Mainstream Open Organizational Course", "Many Open Organic Classes" },
                new string[] { "Ford", "Chevrolet", "Toyota", "BMW" },
                new string[] { "Breaking Bad", "Friends", "The Office", "Seinfeld" },
                new string[] { "Sigmund Freud", "B.F. Skinner", "Carl Jung", "Jean Piaget" },
                new string[] { "Energetika z větru a slunce", "Jaderná energie", "Ropná energie", "Uhelná energie" },
                new string[] { "Milán", "Řím", "Florencie", "Benátky" },
                new string[] { "David", "Guernica", "Poslední večeře", "Hvězdná noc" },
                new string[] { "Burj Khalifa", "Eiffelova věž", "Big Ben", "CN Tower" },
                new string[] { "Čína", "Japonsko", "Koreja", "Thajsko" },
                new string[] { "1939", "1914", "1945", "1923" },
                new string[] { "Analýza silných a slabých stránek", "Statistická analýza", "Ekonomická analýza", "Analýza trhu" },
                new string[] { "Angličtina", "Francouzština", "Mandarínština", "Španělština" },
                new string[] { "Eliud Kipchoge", "Usain Bolt", "Mo Farah", "Haile Gebrselassie" },
                new string[] { "Babička", "Kytice", "Osudy dobrého vojáka Švejka", "Máj" }
            };

            // Index správné odpovědi pro každou otázku (0-indexováno)
            int[] correctAnswerIndexes = new int[]
            {
                2,  // "2"
                0,  // "Karel IV."
                0,  // "Lidské tělo"
                1,  // "Karel Hynek Mácha"
                2,  // "Paříž"
                2,  // "11"
                0,  // "The Beatles"
                0,  // "Kmotr"
                0,  // "Jazyk počítače"
                0,  // "Artificial Intelligence"
                2,  // "Leonardo da Vinci"
                0,  // "Grand Canyon"
                0,  // "Proxima Centauri"
                0,  // "206"
                0,  // "Růst cen"
                1,  // "Rozmanitost"
                0,  // "Immanuel Kant"
                0,  // "Pravidlo ofsajdu"
                0,  // "Obrovská vlna"
                0,  // "Mikročástice"
                1,  // "Zeus"
                0,  // "5 porcí"
                0,  // "Massive Open Online Course"
                0,  // "Ford"
                0,  // "Breaking Bad"
                1,  // "B.F. Skinner"
                0,  // "Energetika z větru a slunce"
                1,  // "Řím"
                0,  // "David"
                0,  // "Burj Khalifa"
                1,  // "Japonsko"
                0,  // "1939"
                0,  // "Analýza silných a slabých stránek"
                0,  // "Angličtina"
                0,  // "Eliud Kipchoge"
                2   // "Osudy dobrého vojáka Švejka"
            };

            // Seedování kvízů
            modelBuilder.Entity<Quiz>().HasData(
                quizIds.Select((id, index) => new Quiz
                {
                    Id = id,
                    Name = quizTitles[index],
                    Description = quizDescriptions[index],
                    AuthorId = adminUserId
                }).ToArray()
            );

            // Seedování otázek – 1 otázka na kvíz
            modelBuilder.Entity<Question>().HasData(
                Enumerable.Range(0, questionCount).Select(i => new Question
                {
                    Id = questionIds[i],
                    Text = questionTexts[i],
                    QuizId = quizIds[i] // každý kvíz má jednu otázku
                }).ToArray()
            );

            // Seedování odpovědí – pro každou otázku 4 možnosti
            modelBuilder.Entity<Answer>().HasData(
                Enumerable.Range(0, questionCount).SelectMany(i =>
                    Enumerable.Range(0, answersPerQuestion).Select(j => new Answer
                    {
                        Id = answerIds[i * answersPerQuestion + j],
                        Text = answerOptions[i][j],
                        IsCorrect = (j == correctAnswerIndexes[i]),
                        QuestionId = questionIds[i]
                    })
                ).ToArray()
            );


        }
    }
}