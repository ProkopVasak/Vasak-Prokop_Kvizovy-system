using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace QuizSystem.Models
{
    public class Quiz
    {
        // Unikátní identifikátor kvízu
        [Key]
        public Guid Id { get; set; }

        // Název kvízu
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        // Popis kvízu
        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        // Seznam otázek, které kvíz obsahuje
        public List<Question> Questions { get; set; } = new List<Question>();

        // Identifikátor autora kvízu
        public string AuthorId { get; set; }

    }
}