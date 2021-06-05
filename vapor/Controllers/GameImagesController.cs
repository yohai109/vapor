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
    public class GameImagesController : Controller
    {
        private readonly vaporContext _context;

        public GameImagesController(vaporContext context)
        {
            _context = context;
        }

        // GET: GameImages
        public async Task<IActionResult> Index()
        {
            var vaporContext = _context.GameImage.Include(g => g.game);
            return View(await vaporContext.ToListAsync());
        }

        // GET: GameImages/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameImage = await _context.GameImage
                .Include(g => g.game)
                .FirstOrDefaultAsync(m => m.id == id);
            if (gameImage == null)
            {
                return NotFound();
            }

            return View(gameImage);
        }

        // GET: GameImages/Create
        public IActionResult Create()
        {
            ViewData["gameId"] = new SelectList(_context.Game, "id", "id");
            return View();
        }

        // POST: GameImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,imageUrl,gameId")] GameImage gameImage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gameImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["gameId"] = new SelectList(_context.Game, "id", "id", gameImage.gameId);
            return View(gameImage);
        }

        // GET: GameImages/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameImage = await _context.GameImage.FindAsync(id);
            if (gameImage == null)
            {
                return NotFound();
            }
            ViewData["gameId"] = new SelectList(_context.Game, "id", "id", gameImage.gameId);
            return View(gameImage);
        }

        // POST: GameImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("id,imageUrl,gameId")] GameImage gameImage)
        {
            if (id != gameImage.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameImageExists(gameImage.id))
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
            ViewData["gameId"] = new SelectList(_context.Game, "id", "id", gameImage.gameId);
            return View(gameImage);
        }

        // GET: GameImages/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameImage = await _context.GameImage
                .Include(g => g.game)
                .FirstOrDefaultAsync(m => m.id == id);
            if (gameImage == null)
            {
                return NotFound();
            }

            return View(gameImage);
        }

        // POST: GameImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var gameImage = await _context.GameImage.FindAsync(id);
            _context.GameImage.Remove(gameImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameImageExists(string id)
        {
            return _context.GameImage.Any(e => e.id == id);
        }
    }
}
