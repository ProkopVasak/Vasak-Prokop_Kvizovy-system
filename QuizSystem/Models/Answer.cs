using System.ComponentModel.DataAnnotations;

namespace QuizSystem.Models
{
    public class Answer
    {
        // Unikátní identifikátor odpovědi
        [Key]
        public Guid Id { get; set; }


        // Text odpovědi
        [Required]
        public string Text { get; set; }


        // Určuje, zda je odpověď správná nebo ne
        [Required]
        public bool IsCorrect { get; set; }


        // Identifikátor otázky, na kterou odpověď patří
        public Guid QuestionId { get; set; }
    }
}