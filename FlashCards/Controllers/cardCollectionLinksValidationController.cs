using FlashCards.Data;
using FlashCards.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace FlashCards.Controllers
{
    public class cardCollectionLinksValidationController : Controller
    {
        private readonly ApplicationDbContext _contex;

        public cardCollectionLinksValidationController(ApplicationDbContext context)
        {
            _contex = context;
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckNewCardId(int cardId)
        {
            var result = _contex.CardCollectionLinks.Any(a => int.Equals(a.CardId, cardId));
            return Json(!result);

        }
        
        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckNewCollectionId(int collectionID)
        {

            var result = _contex.CardCollectionLinks.Any(a => a.CollectionID == collectionID);

            return Json(!result);

        }
    }
}
