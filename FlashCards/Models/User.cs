namespace FlashCards.Models
{
    public class User
    {

        // Ідентифікатор користувача
        public int Id{ get; set; }

        // Нікнейм користувача
        public string nickName { get; set; }

        public ICollection<UserCollectionLink> UserCollectionLinks { get; set; }
        public ICollection<Status> Statuses { get; set; }

    }
}
