using System.ComponentModel.DataAnnotations.Schema;

namespace FlashCards.Models
{
    public class userCollectionLink
    {
        //ід
        public int Id { get; set; }
        //ід користувача
        public string UserId { get; set; }
        //ід колекції
        public int CollectionID { get; set; }

        public Collection? Collection { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }


    }
}
