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
    public class StatusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Status
        public async Task<IActionResult> Index()
        {
            var statuses = await _context.Statuses
                .Include(s => s.Card)
                .Include(s => s.User)
                .ToListAsync();

            return View(statuses);
        }

        // GET: Status/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status = await _context.Statuses
                .Include(s => s.Card)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (status == null)
            {
                return NotFound();
            }

            return View(status);
        }

        // GET: Status/Create
        public IActionResult Create()
        {
            ViewData["CardId"] = new SelectList(_context.Cards, "Id", "Id");
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Status/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,CardId,Mentions,IsWordTakenForLearning,IsWordLearned,IsWordKnownBefore")] Status status)
        {
            if (ModelState.IsValid)
            {
                _context.Add(status);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CardId"] = new SelectList(_context.Cards, "Id", "Id", status.CardId);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id", status.Id);
            return View(status);
        }

        // GET: Status/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status = await _context.Statuses.FindAsync(id);
            if (status == null)
            {
                return NotFound();
            }
            ViewData["CardId"] = new SelectList(_context.Cards, "Id", "Id", status.CardId);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id", status.Id);
            return View(status);
        }

        // POST: Status/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,CardId,Mentions,IsWordTakenForLearning,IsWordLearned,IsWordKnownBefore")] Status status)
        {
            if (id != status.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(status);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusExists(status.Id))
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
            ViewData["CardId"] = new SelectList(_context.Cards, "Id", "Id", status.CardId);
            ViewData["Id"] = new SelectList(_context.Users, "Id", "Id", status.Id);
            return View(status);
        }

        // GET: Status/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status = await _context.Statuses
                .Include(s => s.Card)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (status == null)
            {
                return NotFound();
            }

            return View(status);
        }

        // POST: Status/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var status = await _context.Statuses.FindAsync(id);
            if (status != null)
            {
                _context.Statuses.Remove(status);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatusExists(int id)
        {
            return _context.Statuses.Any(e => e.Id == id);
        }
    }
}
