namespace FlashCards.Models
{
    public class userCollectionLink
    {
        //ід
        public int userCollectionLinkId { get; set; }
        //ід користувача
        public int UserId { get; set; }
        //ід колекції
        public int CollectionID { get; set; }

        public Collection Collection { get; set; }

        public User User { get; set; }
    }
}
