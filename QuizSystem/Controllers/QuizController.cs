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
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : Controller
    {
        private readonly AppDbContext _context;

        // Konstruktor pro závislosti
        public QuizController(AppDbContext context)
        {
            _context = context;
        }

        // Získá všechny kvízy z databáze

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetQuizzes()
        {
            return await _context.Quizzes.ToListAsync();
        }

        // Získá konkrétní kvíz podle ID, včetně jeho otázek a odpovědí

        [HttpGet("{id}")]
        public async Task<ActionResult<Quiz>> GetQuiz(Guid id)
        {
            var quiz = await _context.Quizzes
                   .Include(q => q.Questions)
                   .ThenInclude(q => q.Answers)
                   .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null)
            {
                return NotFound();
            }

            return quiz;
        }

        // Vytvoří nový kvíz v databázi

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Quiz>> CreateQuiz([FromBody] QuizDto quizDto)
        {
            var authorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (authorId == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            var quiz = new Quiz
            {
                Id = Guid.NewGuid(),
                Name = quizDto.Name,
                Description = quizDto.Description,
                AuthorId = authorId,
                Questions = quizDto.Questions.Select(q => new Question
                {
                    Id = Guid.NewGuid(),
                    Text = q.Text,
                    Answers = q.Answers.Select(a => new Answer
                    {
                        Id = Guid.NewGuid(),
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                }).ToList()
            };

            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQuiz), new { id = quiz.Id }, quiz);
        }

        // Aktualizuje existující kvíz v databázi

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateQuiz(Guid id, [FromBody] QuizDto quizDto)
        {
            Console.WriteLine($"Received request to update quiz with ID: {id}");

            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz != null)
            {
                Console.WriteLine($"Removing existing quiz with ID: {id}");
                _context.Questions.RemoveRange(quiz.Questions);
                _context.Quizzes.Remove(quiz);
            }

            // Retrieve the AuthorId from the original quiz or the logged-in user
            var authorId = quiz?.AuthorId ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(authorId))
            {
                Console.WriteLine("AuthorId is missing.");
                return Unauthorized("AuthorId is required.");
            }

            Console.WriteLine("Adding updated quiz from DTO...");
            var newQuiz = new Quiz
            {
                Id = id,
                Name = quizDto.Name,
                Description = quizDto.Description,
                AuthorId = authorId, // Ensure AuthorId is set here
                Questions = quizDto.Questions.Select(q => new Question
                {
                    Id = q.Id ?? Guid.NewGuid(),
                    Text = q.Text,
                    Answers = q.Answers.Select(a => new Answer
                    {
                        Id = a.Id ?? Guid.NewGuid(),
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                }).ToList()
            };

            _context.Quizzes.Add(newQuiz);

            try
            {
                Console.WriteLine("Saving changes to the database...");
                await _context.SaveChangesAsync();
                Console.WriteLine("Changes saved successfully.");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Database update error: " + ex.InnerException?.Message);
                return Conflict(new { Message = "Database update failed.", Details = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }

            Console.WriteLine("UpdateQuiz operation completed successfully.");
            return NoContent();
        }

        // Vygeneruje novou prázdou otázku pro existující kvíz

        [Authorize]
        [HttpPost("generate-question/{quizId}")]
        public IActionResult GenerateQuestion(Guid quizId)
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

        // Odstraní kvíz podle ID

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteQuiz(Guid id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);

            if (quiz == null)
            {
                return NotFound();
            }

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Odesílání odpovědí na kvíz a vyhodnocení výsledků

        [HttpPost("{id}/submit")]
        public async Task<ActionResult<QuizResult>> SubmitQuiz(Guid id, [FromBody] List<QuestionDto> answers)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

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

        // Získá kvízy, které vytvořil aktuálně přihlášený uživatel

        [HttpGet("my")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetMyQuizzes()
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

        // Filtruje kvízy podle zadaného textu

        [HttpGet("filter")]
        [Authorize]
        public async Task<IActionResult> FilterQuizzes([FromQuery] string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return BadRequest(new { Message = "Filter string cannot be empty." });
            }

            Console.WriteLine($"Filtering quizzes with filter: {filter}");

            var filteredQuizzes = await _context.Quizzes
                .Where(q => q.Name.Contains(filter) || q.Description.Contains(filter))
                .ToListAsync();

            if (filteredQuizzes.Count == 0)
            {
                Console.WriteLine($"No quizzes found with filter: {filter}");
                return NotFound(new { Message = "No quizzes found matching the filter." });
            }

            Console.WriteLine($"Found {filteredQuizzes.Count} quizzes matching the filter.");

            return Ok(filteredQuizzes);
        }

    }
}
