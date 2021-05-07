using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using vapor.Data;
using vapor.Models;

namespace vapor.Controllers
{
    public class FollowsController : Controller
    {
        private readonly vaporContext _context;

        public FollowsController(vaporContext context)
        {
            _context = context;
        }

        // GET: Follows
        public async Task<IActionResult> Index()
        {
            var vaporContext = _context.Follow.Include(f => f.followedCustomer).Include(f => f.followingCustomer);
            return View(await vaporContext.ToListAsync());
        }

        // GET: Follows/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var follow = await _context.Follow
                .Include(f => f.followedCustomer)
                .Include(f => f.followingCustomer)
                .FirstOrDefaultAsync(m => m.followingCustomerId == id);
            if (follow == null)
            {
                return NotFound();
            }

            return View(follow);
        }

        // GET: Follows/Create
        public IActionResult Create()
        {
            ViewData["followedCustomerId"] = new SelectList(_context.Customer, "id", "id");
            ViewData["followingCustomerId"] = new SelectList(_context.Customer, "id", "id");
            return View();
        }

        // POST: Follows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("followingCustomerId,followedCustomerId")] Follow follow)
        {
            if (ModelState.IsValid)
            {
                _context.Add(follow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["followedCustomerId"] = new SelectList(_context.Customer, "id", "id", follow.followedCustomerId);
            ViewData["followingCustomerId"] = new SelectList(_context.Customer, "id", "id", follow.followingCustomerId);
            return View(follow);
        }

        // GET: Follows/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var follow = await _context.Follow.FindAsync(id);
            if (follow == null)
            {
                return NotFound();
            }
            ViewData["followedCustomerId"] = new SelectList(_context.Customer, "id", "id", follow.followedCustomerId);
            ViewData["followingCustomerId"] = new SelectList(_context.Customer, "id", "id", follow.followingCustomerId);
            return View(follow);
        }

        // POST: Follows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("followingCustomerId,followedCustomerId")] Follow follow)
        {
            if (id != follow.followingCustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(follow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FollowExists(follow.followingCustomerId))
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
            ViewData["followedCustomerId"] = new SelectList(_context.Customer, "id", "id", follow.followedCustomerId);
            ViewData["followingCustomerId"] = new SelectList(_context.Customer, "id", "id", follow.followingCustomerId);
            return View(follow);
        }

        // GET: Follows/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var follow = await _context.Follow
                .Include(f => f.followedCustomer)
                .Include(f => f.followingCustomer)
                .FirstOrDefaultAsync(m => m.followingCustomerId == id);
            if (follow == null)
            {
                return NotFound();
            }

            return View(follow);
        }

        // POST: Follows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var follow = await _context.Follow.FindAsync(id);
            _context.Follow.Remove(follow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FollowExists(string id)
        {
            return _context.Follow.Any(e => e.followingCustomerId == id);
        }
    }
}
