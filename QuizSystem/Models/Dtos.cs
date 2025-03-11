namespace QuizSystem.Models
{

    // datové přenosové objekty

    public class QuizDto
    {
        // Název kvízu
        public string Name { get; set; }

        // Popis kvízu
        public string Description { get; set; }

        // Seznam otázek, které kvíz obsahuje
        public List<QuestionDto> Questions { get; set; }
    }

    public class QuestionDto
    {
        // Volitelný identifikátor otázky (může být null při vytváření nové otázky)
        public Guid? Id { get; set; }

        // Text otázky
        public string Text { get; set; }

        // Seznam odpovědí k této otázce
        public List<AnswerDto> Answers { get; set; }
    }

    public class AnswerDto
    {

        // Volitelný identifikátor odpovědi (může být null při vytváření nové odpovědi)
        public Guid? Id { get; set; }

        // Text odpovědi
        public string Text { get; set; }

        // Určuje, zda je odpověď správná
        public bool IsCorrect { get; set; }
    }

    public class ChangeRoleRequest
    {

        // ID uživatele, jehož role se má změnit
        public string UserId { get; set; }

        // Nová role, kterou má uživatel získat
        public string NewRole { get; set; }
    }
}