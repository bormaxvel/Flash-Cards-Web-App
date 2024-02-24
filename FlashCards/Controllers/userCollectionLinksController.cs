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
    public class userCollectionLinksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public userCollectionLinksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: userCollectionLinks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserCollectionLinks.Include(u => u.Collection).Include(u => u.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: userCollectionLinks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCollectionLink = await _context.UserCollectionLinks
                .Include(u => u.Collection)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userCollectionLink == null)
            {
                return NotFound();
            }

            return View(userCollectionLink);
        }

        // GET: userCollectionLinks/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(_context.Collections, "Id", "Id");
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: userCollectionLinks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,CollectionID")] userCollectionLink userCollectionLink)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userCollectionLink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Collections, "Id", "Id", userCollectionLink.Id);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id", userCollectionLink.Id);
            return View(userCollectionLink);
        }

        // GET: userCollectionLinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCollectionLink = await _context.UserCollectionLinks.FindAsync(id);
            if (userCollectionLink == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Collections, "Id", "Id", userCollectionLink.Id);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id", userCollectionLink.Id);
            return View(userCollectionLink);
        }

        // POST: userCollectionLinks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,CollectionID")] userCollectionLink userCollectionLink)
        {
            if (id != userCollectionLink.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userCollectionLink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!userCollectionLinkExists(userCollectionLink.Id))
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
            ViewData["Id"] = new SelectList(_context.Collections, "Id", "Id", userCollectionLink.Id);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id", userCollectionLink.Id);
            return View(userCollectionLink);
        }

        // GET: userCollectionLinks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCollectionLink = await _context.UserCollectionLinks
                .Include(u => u.Collection)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userCollectionLink == null)
            {
                return NotFound();
            }

            return View(userCollectionLink);
        }

        // POST: userCollectionLinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userCollectionLink = await _context.UserCollectionLinks.FindAsync(id);
            if (userCollectionLink != null)
            {
                _context.UserCollectionLinks.Remove(userCollectionLink);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool userCollectionLinkExists(int id)
        {
            return _context.UserCollectionLinks.Any(e => e.Id == id);
        }
    }
}
