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
   
   
    public class userCollectionLinksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public userCollectionLinksController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: userCollectionLinks
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var isAdmin = await _userManager.IsInRoleAsync(currentUser, RoleNames.ADMIN);

            IQueryable<userCollectionLink> userCollectionLinks;

            if (isAdmin)
            {
                userCollectionLinks = _context.UserCollectionLinks
                    .Include(u => u.Collection)
                    .Include(u => u.User);
            }
            else
            {
                userCollectionLinks = _context.UserCollectionLinks
                    .Include(u => u.Collection)
                    .Include(u => u.User)
                    .Where(u => u.UserId == currentUser.Id);
            }

            return View(await userCollectionLinks.ToListAsync());
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
        [Authorize(Roles = RoleNames.ADMIN)]
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["CollectionID"] = new SelectList(_context.Collections, "Id", "Name");
            return PartialView("~/Views/userCollectionLinks/_AddNewLink.cshtml");
        }

        // POST: userCollectionLinks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> Create([Bind("Id,UserId,CollectionID")] userCollectionLink userCollectionLink)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userCollectionLink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
     //       ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", userCollectionLink.UserId);
       //     ViewData["CollectionID"] = new SelectList(_context.Collections, "Id", "Name", userCollectionLink.CollectionID);
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
        [Authorize(Roles = RoleNames.ADMIN)]
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
        [Authorize(Roles = RoleNames.ADMIN)]
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
