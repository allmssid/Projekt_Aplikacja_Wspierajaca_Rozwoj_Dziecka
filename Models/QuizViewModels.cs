namespace Aplikacja_wspierajaca_rozwoj_dziecka.Models
{
    // Matematyka
    public class MathQuizQuestionViewModel
    {
        public int A { get; set; }
        public int B { get; set; }
        public string Operation { get; set; } = "+";   // +, -, *
        public int? UserAnswer { get; set; }
    }

    public class MathQuizViewModel
    {
        public QuizDifficulty Difficulty { get; set; }
        public List<MathQuizQuestionViewModel> Questions { get; set; } = new();
    }

    // Angielski
    public class EnglishQuizQuestionViewModel
    {
        public string Polish { get; set; } = "";
        public string English { get; set; } = "";     // poprawna odp.
        public string? UserAnswer { get; set; }
    }

    public class EnglishQuizViewModel
    {
        public QuizDifficulty Difficulty { get; set; }
        public List<EnglishQuizQuestionViewModel> Questions { get; set; } = new();
    }
}
