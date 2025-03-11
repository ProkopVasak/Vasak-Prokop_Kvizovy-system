using System.ComponentModel.DataAnnotations;

namespace QuizSystem.Models
{
    public class Question
    {
        // Unikátní identifikátor otázky
        [Key]
        public Guid Id { get; set; }

        // Text otázky
        [Required]
        [StringLength(100)]
        public string Text { get; set; }

        // Seznam odpovědí, které patří k této otázce
        public List<Answer> Answers { get; set; } = new List<Answer>();

        // Identifikátor kvízu, ke kterému otázka patří (odkazuje na Quiz model)
        public Guid QuizId { get; set; }
    }
}