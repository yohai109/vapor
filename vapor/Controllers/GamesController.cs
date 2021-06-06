using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using vapor.Data;
using vapor.Models;

namespace vapor.Controllers
{
    public class GamesController : Controller
    {
        private readonly vaporContext _context;

        public GamesController(vaporContext context)
        {
            _context = context;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            return View(await _context.Game.ToListAsync());
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.Include(g => g.images).FirstOrDefaultAsync(m => m.id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,price,name,description,releaseDate")] Game game,
                                                List<IFormFile> gameImages)
        {
            if (ModelState.IsValid)
            {
                GameImage gameImage;
                game.images = new List<GameImage>();

                // Saves all the new images
                foreach (IFormFile image in gameImages)
                {
                    using (var ms = new MemoryStream())
                    {
                        image.CopyTo(ms);
                        byte[] fileBytes = ms.ToArray();

                        gameImage = new GameImage();
                        gameImage.fileBase64 = Convert.ToBase64String(fileBytes);
                        gameImage.fileContentType = image.ContentType;
                        game.images.Add(gameImage);
                    }
                }

                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }


        [HttpGet]
        public async Task<IActionResult> GetGameImage(string imageId)
        {
            GameImage gameImage = await _context.GameImage.FirstOrDefaultAsync(gi => gi.id == imageId);
            byte[] fileBytes = Convert.FromBase64String(gameImage.fileBase64);
            return this.File(fileBytes, gameImage.fileContentType);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("id,price,name,description,releaseDate")] Game game)
        {
            if (id != game.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.id))
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
            return View(game);
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.Include(g => g.images).FirstOrDefaultAsync(m => m.id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var game = await _context.Game.Include(g => g.images).FirstOrDefaultAsync(m => m.id == id);
            _context.Game.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(string id)
        {
            return _context.Game.Any(e => e.id == id);
        }
        public IActionResult Test()
        {
            return View();
        }
    }
}
