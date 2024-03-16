using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlashCards.Data;
using FlashCards.Models;
using Microsoft.AspNetCore.Authorization;

namespace FlashCards.Controllers
{
    [Authorize]
    public class cardCollectionLinksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public cardCollectionLinksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: cardCollectionLinks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CardCollectionLinks.Include(c => c.Card).Include(c => c.Collection);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: cardCollectionLinks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cardCollectionLink = await _context.CardCollectionLinks
                .Include(c => c.Card)
                .Include(c => c.Collection)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cardCollectionLink == null)
            {
                return NotFound();
            }

            return View(cardCollectionLink);
        }

        // GET: cardCollectionLinks/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(_context.Cards, "Id", "Id");
            ViewData["Id"] = new SelectList(_context.Collections, "Id", "Id");
            return View();
        }

        // POST: cardCollectionLinks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CardId,CollectionID")] cardCollectionLink cardCollectionLink)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cardCollectionLink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Cards, "Id", "Id", cardCollectionLink.Id);
            ViewData["Id"] = new SelectList(_context.Collections, "Id", "Id", cardCollectionLink.Id);
            return View(cardCollectionLink);
        }

        // GET: cardCollectionLinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cardCollectionLink = await _context.CardCollectionLinks.FindAsync(id);
            if (cardCollectionLink == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Cards, "Id", "Id", cardCollectionLink.Id);
            ViewData["Id"] = new SelectList(_context.Collections, "Id", "Id", cardCollectionLink.Id);
            return View(cardCollectionLink);
        }

        // POST: cardCollectionLinks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CardId,CollectionID")] cardCollectionLink cardCollectionLink)
        {
            if (id != cardCollectionLink.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cardCollectionLink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!cardCollectionLinkExists(cardCollectionLink.Id))
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
            ViewData["Id"] = new SelectList(_context.Cards, "Id", "Id", cardCollectionLink.Id);
            ViewData["Id"] = new SelectList(_context.Collections, "Id", "Id", cardCollectionLink.Id);
            return View(cardCollectionLink);
        }

        // GET: cardCollectionLinks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cardCollectionLink = await _context.CardCollectionLinks
                .Include(c => c.Card)
                .Include(c => c.Collection)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cardCollectionLink == null)
            {
                return NotFound();
            }

            return View(cardCollectionLink);
        }

        // POST: cardCollectionLinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cardCollectionLink = await _context.CardCollectionLinks.FindAsync(id);
            if (cardCollectionLink != null)
            {
                _context.CardCollectionLinks.Remove(cardCollectionLink);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool cardCollectionLinkExists(int id)
        {
            return _context.CardCollectionLinks.Any(e => e.Id == id);
        }
    }
}
