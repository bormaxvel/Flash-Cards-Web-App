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
    public class CardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cards
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cards.ToListAsync());
        }

        // GET: Cards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        // POST: Cards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Term,Definition,Context")] Card card)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(card);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(card);
        //}

        // GET: Cards/Create
        public IActionResult Create()
        {
            // Fetch the available collection names from the database
            var collectionNames = _context.Collections.Select(c => c.Name).ToList();

            // Pass the collection names to the view as a SelectList
            ViewData["CollectionNames"] = new SelectList(collectionNames);

            return View();
        }

        // POST: Cards/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Term,Definition,Context")] Card card)
        {
            if (ModelState.IsValid)
            {
                // Check if the card exists in the database
                var existingCard = await _context.Cards.FirstOrDefaultAsync(c => c.Id == card.Id);

                if (existingCard == null)
                {
                    // Add the card to the Card table if it doesn't exist
                    _context.Add(card);
                    await _context.SaveChangesAsync(); // Save changes to get the newly generated card Id
                }
                else
                {
                    // Update the existing card with the new data
                    _context.Entry(existingCard).CurrentValues.SetValues(card);
                    await _context.SaveChangesAsync(); // Save changes to update the existing card
                }

                // Retrieve the corresponding collection based on the context (Name)
                var collection = await _context.Collections.FirstOrDefaultAsync(c => c.Name == card.Context);

                if (collection != null)
                {
                    // Create a new cardCollectionLink entry
                    var cardCollectionLink = new cardCollectionLink
                    {
                        CardId = card.Id,
                        CollectionID = collection.Id
                    };

                    // Add the cardCollectionLink to the table
                    _context.CardCollectionLinks.Add(cardCollectionLink);
                    await _context.SaveChangesAsync(); // Save changes to add the new link
                }

                return RedirectToAction(nameof(Index));
            }
            return View(card);
        }




        // GET: Cards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }
            return View(card);
        }

        // POST: Cards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Term,Definition,Context")] Card card)
        {
            if (id != card.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(card);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardExists(card.Id))
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
            return View(card);
        }

        // GET: Cards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        // POST: Cards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card != null)
            {
                _context.Cards.Remove(card);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CardExists(int id)
        {
            return _context.Cards.Any(e => e.Id == id);
        }
    }
}
