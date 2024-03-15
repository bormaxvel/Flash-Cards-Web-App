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

namespace FlashCards.Controllers
{
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
            }
            else if (!isChecked && link != null)
            {
                // Удаляем запись из таблицы userCollectionLink
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
            return View();
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
            return View(collection);
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
