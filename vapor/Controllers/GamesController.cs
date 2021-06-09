using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            /*var vaporContext = _context.Game
                .Include(g => g.generes)
                .Select(game => new
                {
                    game,
                    images = game.images.Select(i => new GameImage { id = i.id }).First()
                });
*/
            var vaporContext = _context.Game
                .Include(g => g.generes)
                .Include(g => g.developer)
                .Include(g => g.images);

            return View(await vaporContext.ToListAsync());
        }

        // GET: Games/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var loadedGame = await _context.Game
                .Select(game => new
                {
                    game,
                    images = game.images.Select(i => new GameImage { id = i.id }).ToList()
                })
                .FirstOrDefaultAsync(g => g.game.id == id);

            if (loadedGame == null)
            {
                return NotFound();
            }

            loadedGame.game.images = loadedGame.images;

            return View(loadedGame.game);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            ViewData["developerId"] = new SelectList(_context.Developer, "id", "name");
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
            ViewData["developerId"] = new SelectList(_context.Developer, "id", "id", game.developerId);
            return View(game);
        }


        [HttpGet]
        public async Task<ActionResult> GetGameImage(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GameImage gameImage = await _context.GameImage.FirstOrDefaultAsync(gi => gi.id == id);

            if (gameImage == null)
            {
                return NotFound();
            }

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
            ViewData["developerId"] = new SelectList(_context.Developer, "id", "id", game.developerId);
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("id,developerId,price,name,description,releaseDate")] Game game)
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
            ViewData["developerId"] = new SelectList(_context.Developer, "id", "id", game.developerId);
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string query)
        {
            var searchResult = _context.Game
                .Include(g => g.generes)
                .Include(g => g.developer)
                .Where(g => ( query != "" && query != null ) ? g.name.Contains(query) : true)
                .Select(g => new
                {
                    id = g.id,
                    name = g.name,
                    developer = g.developer.name,
                    generes = g.generes,
                    imageid = g.images.FirstOrDefault().id
                });

            return Json(await searchResult.ToListAsync());
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
