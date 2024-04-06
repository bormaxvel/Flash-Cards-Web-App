using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlashCards.Data;
using FlashCards.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FlashCards.Controllers
{
    [Authorize]
    public class CollectionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CollectionsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> ToggleMention(int cardId, bool remembered)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var status = await _context.Statuses.FirstOrDefaultAsync(s => s.UserId == currentUser.Id && s.CardId == cardId);

            if (status != null)
            {
                if (remembered)
                {
                    status.Mentions++;
                }
                else
                {
                    status.Mentions--;
                }

                await _context.SaveChangesAsync();
                return Ok();
            }

            return NotFound();
        }

        public async Task<IActionResult> ViewCards(int collectionId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var cardsInCollection = await _context.CardCollectionLinks
                .Where(ccl => ccl.CollectionID == collectionId)
                .Select(ccl => ccl.Card)
                .ToListAsync();

            return View(cardsInCollection);
        }



        [HttpPost]
        public async Task<IActionResult> ToggleAccess(int collectionId, bool isChecked)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var link = await _context.UserCollectionLinks.FirstOrDefaultAsync(ucl => ucl.CollectionID == collectionId && ucl.UserId == currentUser.Id);

            if (isChecked && link == null)
            {
                // Добавляем запись в таблицу userCollectionLink
                var newLink = new userCollectionLink
                {
                    UserId = currentUser.Id,
                    CollectionID = collectionId
                };
                _context.UserCollectionLinks.Add(newLink);
                var wordsInCollection = await _context.CardCollectionLinks
           .Where(ccl => ccl.CollectionID == collectionId)
           .Select(ccl => ccl.Card)
           .ToListAsync();

                // Додаємо записи в таблицю Status для кожного слова
                foreach (var word in wordsInCollection)
                {
                    var status = new Status
                    {
                        UserId = currentUser.Id,
                        CardId = word.Id,
                        Mentions = 0,
                        IsWordTakenForLearning = true,
                        IsWordLearned = false,
                        IsWordKnownBefore = false
                        // Інші значення статусу можна задати за замовчуванням або відповідно до вашої логіки
                    };
                    _context.Statuses.Add(status);
                }
            }
            else if (!isChecked && link != null)
            {
                var wordsInCollectionIds = await _context.CardCollectionLinks
                .Where(ccl => ccl.CollectionID == collectionId)
                .Select(ccl => ccl.CardId)
                .ToListAsync();

                // Отримуємо всі записи з таблиці Status, які пов'язані з користувачем та словами з колекції
                var statusesToRemove = await _context.Statuses
                    .Where(s => s.UserId == currentUser.Id && wordsInCollectionIds.Contains(s.CardId))
                    .ToListAsync();

                // Видаляємо ці записи з таблиці Status
                _context.Statuses.RemoveRange(statusesToRemove);


                _context.UserCollectionLinks.Remove(link);
            }
            

           

            await _context.SaveChangesAsync();
            return Ok();
        }

        // GET: Collections
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Получаем все коллекции из базы данных
            var collections = await _context.Collections.ToListAsync();

            // Создаем словарь, где ключ - это ID коллекции, а значение - указывает, имеет ли пользователь доступ к этой коллекции
            var collectionAccess = new Dictionary<int, bool>();
            foreach (var collection in collections)
            {
                // Проверяем наличие записи в таблице userCollectionLink для текущего пользователя и коллекции
                var link = await _context.UserCollectionLinks.FirstOrDefaultAsync(ucl => ucl.CollectionID == collection.Id && ucl.UserId == currentUser.Id);
                collectionAccess.Add(collection.Id, link != null);
            }

            // Передаем коллекции и информацию о доступе к представлению
            ViewData["CollectionAccess"] = collectionAccess;
            return View(collections);
        }

        // GET: Collections/Details/5
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

        // GET: Collections/Create
        public IActionResult Create()
        {
            //return View();
            return PartialView("~/Views/Collections/_AddNewCollection.cshtml");
        }

        // POST: Collections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Collection collection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(collection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //return View(collection);
            return PartialView("~/Views/Collections/_AddNewCollection.cshtml");
        }

        // GET: Collections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collection = await _context.Collections.FindAsync(id);
            if (collection == null)
            {
                return NotFound();
            }
            return View(collection);
        }

        // POST: Collections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Collection collection)
        {
            if (id != collection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(collection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CollectionExists(collection.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(collection);
        }

        // GET: Collections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collection = await _context.Collections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collection == null)
            {
                return NotFound();
            }

            return View(collection);
        }

        // POST: Collections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Response.Cookies.Append("cookieName", "cookieValue", new CookieOptions
            {
                Secure = Request.IsHttps
            });

            var collection = await _context.Collections.FindAsync(id);
            if (collection != null)
            {
                _context.Collections.Remove(collection);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CollectionExists(int id)
        {
            return _context.Collections.Any(e => e.Id == id);
        }

    }
}
