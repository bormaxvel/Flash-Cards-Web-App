using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlashCards.Models
{
    public class userCollectionLink
    {
        //ід
        [Key]
        public int Id { get; set; }
        //ід користувача
        [Display(Name = "uclUserId")]
        
        public string UserId { get; set; }
        
        //ід колекції
        [Display(Name = "uclCollectionId")]
        
        public int CollectionID { get; set; }

        [Display(Name = "uclCollection")]
        public Collection? Collection { get; set; }

        [Display(Name = "uclUser")]
        [ForeignKey("UserId")]
        public User? User { get; set; }


    }
}
