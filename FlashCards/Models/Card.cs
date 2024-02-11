namespace FlashCards.Models
{
    public class Card
    {

        // Ідентифікатор картки
        public int Id { get; set; }
        // Термін (оригінал слова)
        public string Term { get; set; }
        // Визначення (переклад)
        public string Definition { get; set; }
        // Контекст (як використовується)
        public string Context { get; set; }

    }
}