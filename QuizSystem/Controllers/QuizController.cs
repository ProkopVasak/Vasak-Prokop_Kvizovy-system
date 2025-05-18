using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using QuizSystem.Models;
using QuizSystem.Services;

namespace QuizSystem.Controllers
{
    [Route("api/quizzes")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Konstruktor pro závislosti
        public QuizController(AppDbContext context)
        {
            _context = context;
        }

        // Získá všechny kvízy z databáze

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetAll()
        {
            return await _context.Quizzes.ToListAsync();
        }

        // Získá kvízy, které vytvořil aktuálně přihlášený uživatel

        [HttpGet("mine")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetMine()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            var quizzes = await _context.Quizzes
                .Where(q => q.AuthorId == userId)
                .ToListAsync();

            return quizzes;
        }

        // Získá konkrétní kvíz podle ID, včetně jeho otázek a odpovědí

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Quiz>> GetById(Guid id)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null) return NotFound();
            return Ok(quiz);
        }

        // Vytvoří nový kvíz v databázi

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Quiz>> Create([FromBody] QuizDto quizDto)
        {
            // Validace: minimálně jedna otázka
            if (quizDto.Questions == null || !quizDto.Questions.Any())
                return BadRequest(new { Message = "Kvíz musí obsahovat alespoň jednu otázku." });

            // Validace: každá otázka musí mít právě jednu správnou odpověď
            foreach (var q in quizDto.Questions)
            {
                var correctCount = q.Answers?.Count(a => a.IsCorrect) ?? 0;
                if (correctCount != 1)
                    return BadRequest(new
                    {
                        Message =
                          $"Otázka '{q.Text}' musí mít právě jednu správnou odpověď, " +
                          $"nalezeno: {correctCount}."
                    });
            }

            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (authorId == null)
                return Unauthorized();

            var quiz = new Quiz
            {
                Id = Guid.NewGuid(),
                Name = quizDto.Name,
                Description = quizDto.Description,
                AuthorId = authorId,
                Questions = quizDto.Questions.Select(q => new Question
                {
                    Id = Guid.NewGuid(),
                    QuizId = Guid.Empty, // EF doplní později
                    Text = q.Text,
                    Answers = q.Answers.Select(a => new Answer
                    {
                        Id = Guid.NewGuid(),
                        QuestionId = Guid.Empty,
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                }).ToList()
            };

            // Korektní nastavení FK
            foreach (var question in quiz.Questions)
            {
                question.QuizId = quiz.Id;
                foreach (var answer in question.Answers)
                    answer.QuestionId = question.Id;
            }

            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = quiz.Id }, quiz);
        }

        // Aktualizuje existující kvíz v databázi

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] QuizDto quizDto)
        {
            // 1) Validace: aspoň jedna otázka
            if (quizDto.Questions == null || !quizDto.Questions.Any())
                return BadRequest(new { Message = "Kvíz musí obsahovat alespoň jednu otázku." });

            // 2) Validace: každá otázka má právě jednu správnou odpověď
            foreach (var q in quizDto.Questions)
            {
                var correctCount = q.Answers?.Count(a => a.IsCorrect) ?? 0;
                if (correctCount != 1)
                    return BadRequest(new
                    {
                        Message =
                          $"Otázka '{q.Text}' musí mít právě jednu správnou odpověď, " +
                          $"nalezeno: {correctCount}."
                    });
            }

            // 3) Autorizace
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // 4) Načtení existujícího kvízu s podřízenými entitami
            var quiz = await _context.Quizzes
              .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
              .FirstOrDefaultAsync(q => q.Id == id);
            if (quiz == null) return NotFound();
            if (quiz.AuthorId != userId) return Forbid();

            // 5) Aktualizace hlavních polí
            quiz.Name = quizDto.Name;
            quiz.Description = quizDto.Description;
            await _context.SaveChangesAsync();

            // 6) Smazání starých otázek + odpovědí
            _context.Questions.RemoveRange(quiz.Questions);
            await _context.SaveChangesAsync();

            // 7) Vložení nových otázek + odpovědí
            var newQuestions = quizDto.Questions.Select(qDto =>
            {
                var qId = qDto.Id ?? Guid.NewGuid();
                return new Question
                {
                    Id = qId,
                    QuizId = quiz.Id,
                    Text = qDto.Text,
                    Answers = qDto.Answers.Select(aDto => new Answer
                    {
                        Id = aDto.Id ?? Guid.NewGuid(),
                        QuestionId = qId,
                        Text = aDto.Text,
                        IsCorrect = aDto.IsCorrect
                    }).ToList()
                };
            }).ToList();
            _context.Questions.AddRange(newQuestions);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Odstraní kvíz podle ID

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            // 1) Získáme ID právě přihlášeného uživatele
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // 2) Načteme kvíz z DB
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null)
                return NotFound();

            // 3) Ověříme, že vlastníkem je právě tento uživatel
            if (quiz.AuthorId != userId)
                return Forbid(); // 403 Forbidden

            // 4) Provedeme smazání
            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id:guid}/edit")]
        [Authorize]
        public async Task<ActionResult<Quiz>> GetForEdit(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var quiz = await _context.Quizzes
              .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
              .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null)
                return NotFound();

            if (quiz.AuthorId != userId)
                return Forbid();

            return Ok(quiz);
        }
    }

    [ApiController]
    [Route("api/quizzes/{quizId:guid}/questions")]
    public class QuestionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public QuestionsController(AppDbContext context) => _context = context;

        // Vygeneruje novou prázdou otázku pro existující kvíz

        [HttpGet("template")]
        [Authorize]
        public IActionResult Create(Guid quizId)
        {
            // Vygenerování nového ID pro otázku
            var questionId = Guid.NewGuid();

            var newQuestion = new Question
            {
                Id = questionId,
                QuizId = quizId, // Nastavení QuizId
                Text = "",
                Answers = new List<Answer>
                {
                    new Answer { Id = Guid.NewGuid(), Text = "", IsCorrect = false, QuestionId = questionId },
                    new Answer { Id = Guid.NewGuid(), Text = "", IsCorrect = false, QuestionId = questionId },
                    new Answer { Id = Guid.NewGuid(), Text = "", IsCorrect = false, QuestionId = questionId },
                    new Answer { Id = Guid.NewGuid(), Text = "", IsCorrect = false, QuestionId = questionId }
                }
            };

            return Ok(newQuestion);
        }
    }

    [ApiController]
    [Route("api/quizzes/{quizId:guid}/submissions")]
    public class SubmissionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public SubmissionsController(AppDbContext context) => _context = context;

        // Odesílání odpovědí na kvíz a vyhodnocení výsledků

        [HttpPost]
        public async Task<ActionResult<QuizResult>> Create(Guid quizId, [FromBody] List<QuestionDto> answers)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null)
            {
                return NotFound();
            }

            var correctAnswers = 0;
            var totalQuestions = quiz.Questions.Count;

            foreach (var question in quiz.Questions)
            {
                var submittedQuestion = answers.FirstOrDefault(q => q.Text == question.Text);

                if (submittedQuestion == null)
                {
                    continue;
                }

                var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);

                if (correctAnswer != null && submittedQuestion.Answers.Any(a => a.Text == correctAnswer.Text))
                {
                    correctAnswers++;
                }
            }

            var quizResult = new QuizResult
            {
                CorrectAnswers = correctAnswers,
                TotalQuestions = totalQuestions
            };

            return quizResult;
        }

    }
}
