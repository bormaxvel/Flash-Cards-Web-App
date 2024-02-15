namespace FlashCards.Models
{
    public class Collection
    {

        // Ідентифікатор колекції
        public int Id { get; set; }

        // Назва колекції
        public string Name { get; set; }

        // Опис колекції
        public string Description { get; set; }

        public ICollection<CardCollectionLink> CardCollectionLinks { get; set; }
        public ICollection<UserCollectionLink> UserCollectionLinks { get; set; }


    }
}
