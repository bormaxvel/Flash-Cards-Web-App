namespace FlashCards.Models
{
    public class cardCollectionLink
    {
        // Ідентифікатор
        public int cardCollectionLinkId { get; set; }
        //ід картки
        public int CardId { get; set; }
        //ід назви
        public int CollectionID { get; set; }

        public Card Card { get; set; }

        public Collection Collection { get; set; }

    }
}
