using FlashCards.Data;
using FlashCards.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FlashCards.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        //З'єднання двох таблиць для подальшої вибірки кількості слів в кожній категорії
        public async Task<IActionResult> Index()
        {
            var collections = await _context.Collections
                .Include(c => c.CardCollectionLinks)
                .ToListAsync();
            return View(collections);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collection = await _context.Collections
                .Include(c => c.CardCollectionLinks)
                .ThenInclude(cc => cc.Card)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (collection == null)
            {
                return NotFound();
            }

            return View(collection);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
