namespace FlashCards.Models
{
    public class Card
    {

        // Ід
        public int Id { get; set; }
        // Оригінальне значення
        public string Term { get; set; }
        // Визначення або переклад
        public string Definition { get; set; }
        // Контекст використання
        public string Context { get; set; }

        public ICollection<cardCollectionLink>? CardCollectionLinks { get; set; }
        public ICollection<Status>? Statuses { get; set; }

    }
}
