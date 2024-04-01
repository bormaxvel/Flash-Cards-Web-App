using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FlashCards.Models
{
    public class User : IdentityUser
    {

        // Ідентифікатор користувача

        // Нікнейм користувача
        [Display(Name = "uNickName")]
        [Required(ErrorMessage = "lblerrorNick")]
        [Remote("CheckNickName", "UsersValidation", ErrorMessage = "lblexist")]
        public string? nickName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "lblerrorEmail")]
        public override string? Email { get; set; }

        public ICollection<userCollectionLink>? UserCollectionLinks { get; set; }
        public ICollection<Status>? Statuses { get; set; }

    }
}
