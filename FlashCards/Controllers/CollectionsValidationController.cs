using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlashCards.Data;
using FlashCards.Models;

namespace FlashCards.Controllers
{
    public class CollectionsValidationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CollectionsValidationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckNewCollectionName(string Name, int id)
        {

            //var result = _context.Collections.Any(a => string.Equals(a.Name, Name));

            //return Json(!result);

            var existingCollection = _context.Collections.FirstOrDefault(c => c.Name == Name);

            bool isNameAvailable = existingCollection == null || existingCollection.Id == id;

            return Json(!isNameAvailable);

        }

    }
}
