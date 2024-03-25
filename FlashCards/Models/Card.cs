using System.ComponentModel.DataAnnotations;

namespace FlashCards.Models
{
    public class Card
    {

        // Ід
        public int Id { get; set; }
        // Оригінальне значення

        [Display(Name = "lblCardName")]
        public string Term { get; set; }
        // Визначення або переклад

        [Display(Name = "lblCardDefinition")]
        public string Definition { get; set; }

        [Display(Name = "lblCardContext")]
        // Контекст використання
        public string Context { get; set; }

        public ICollection<cardCollectionLink>? CardCollectionLinks { get; set; }
        public ICollection<Status>? Statuses { get; set; }

    }
}
