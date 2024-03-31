using System.ComponentModel.DataAnnotations;

namespace FlashCards.Models
{
    public class Card
    {

        // Ід
        public int Id { get; set; }
        // Оригінальне значення

        [Display(Name = "lblCardName")]
        [Required (ErrorMessage = "lblerrorTerm")]
        public string Term { get; set; }
        // Визначення або переклад

        [Display(Name = "lblCardDefinition")]
        [Required(ErrorMessage = "lblerrorDefinition")]
        public string Definition { get; set; }

        [Display(Name = "lblCardContext")]
        [Required]
        // Контекст використання
        public string Context { get; set; }

        public ICollection<cardCollectionLink>? CardCollectionLinks { get; set; }
        public ICollection<Status>? Statuses { get; set; }

    }
}
