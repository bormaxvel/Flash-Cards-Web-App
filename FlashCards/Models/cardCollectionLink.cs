using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FlashCards.Models
{
    public class cardCollectionLink
    {
        // Ідентифікатор
        public int Id { get; set; }
        //ід картки
        //[Required(ErrorMessage = "lblerrorCardId")]
        //[Remote("CheckNewCardId", "cardCollectionLinkValidation", ErrorMessage = "lblexistedCardId")]
        public int? CardId { get; set; }
        //ід назви
        //[Required(ErrorMessage = "lblerrorCollectionID")]
        //[Remote("CheckNewCollectionId", "cardCollectionLinkValidation", ErrorMessage = "lblexistedCollectionId")]
        public int? CollectionID { get; set; }

        public Card? Card { get; set; }

        public Collection? Collection { get; set; }

    }
}
