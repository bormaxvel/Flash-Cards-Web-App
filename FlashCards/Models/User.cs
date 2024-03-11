using Microsoft.AspNetCore.Identity;

namespace FlashCards.Models
{
    public class User : IdentityUser
    {

        // Ідентифікатор користувача

        // Нікнейм користувача
        public string? nickName { get; set; }

        public ICollection<userCollectionLink>? UserCollectionLinks { get; set; }
        public ICollection<Status>? Statuses { get; set; }

    }
}
