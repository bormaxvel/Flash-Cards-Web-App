using FlashCards.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlashCards.Controllers
{
    public class CardsValidationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CardsValidationController(ApplicationDbContext context)
        {
            _context = context;
        }
        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckNewWord(string term, int id)
        {
           
            var existingWord = _context.Cards.FirstOrDefault(a => a.Term == term);
            bool isNameAvailable = existingWord == null || existingWord.Id == id;
            return Json(isNameAvailable);
            
        }
        
    }
}
