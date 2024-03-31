using System.ComponentModel.DataAnnotations;

namespace FlashCards.Models
{
    public class Collection
    {

        // Ідентифікатор колекції
        public int Id { get; set; }

        [Display(Name="lblCollectionName")]
        [Required(ErrorMessage = "lblerrorName")]
        // Назва колекції
        public string Name { get; set; }

        [Display(Name = "CollectionDescription")]
        // Опис колекції
        public string Description { get; set; }

        public ICollection<cardCollectionLink>? CardCollectionLinks { get; set; }
        public ICollection<userCollectionLink>? UserCollectionLinks { get; set; }


    }
}
