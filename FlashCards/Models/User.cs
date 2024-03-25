using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FlashCards.Models
{
    public class User : IdentityUser
    {

        // Ідентифікатор користувача

        // Нікнейм користувача
        [Display(Name = "uNickName")]
        public string? nickName { get; set; }

        public ICollection<userCollectionLink>? UserCollectionLinks { get; set; }
        public ICollection<Status>? Statuses { get; set; }

    }
}
