namespace FlashCards.Models
{
    public class Status
    {
        // Ідентифікатор
        public int StatusId { get; set; }

        // Ідентифікатор користувача
        public int UserId { get; set; }

        // Ідентифікатор слова
        public int CardId { get; set; }

        // Кількість згадок (скільки разів поспіль користувач згадав слово)
        public int Mentions { get; set; }

        // Чи було слово взято для вивчення?
        public bool IsWordTakenForLearning { get; set; }

        // Чи вивчено слово?
        public bool IsWordLearned { get; set; }

        // Чи було слово відоме раніше?
        public bool IsWordKnownBefore { get; set; }

    }
}
