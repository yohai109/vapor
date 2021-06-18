using System;
using System.Collections.Generic;
using System.Dynamic;
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
using System.ServiceModel.Syndication;
using System.Xml;
using vapor.services;

namespace vapor.Controllers
{
    public class GamesController : Controller
    {
        private readonly vaporContext _context;
        private twitter _twitterService;

        public GamesController(vaporContext context, twitter twitterService)
        {
            _context = context;
            _twitterService = twitterService;
        }

        // GET: Games
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {

            var games = _context.Game
                .Include(g => g.generes)
                .Include(g => g.developer)
                .Include(g => g.images)
                .Select(g => new Game
                {
                    id = g.id,
                    name = g.name,
                    developer = g.developer,
                    generes = (ICollection<Genre>)g.generes.Take(4),
                    images = new List<GameImage>()
                    {
                        g.images.Select(i => new GameImage { id = i.id }).First()
                    }
                }).ToListAsync();



            dynamic model = new ExpandoObject();
            model.games = await games;

            var genres = _context.Genre.ToArrayAsync();
            model.genres = await genres;

            var developers = _context.Developer.ToArrayAsync();
            model.developers = await developers;

            return View(model);
        }

        [HttpGet]
        public List<String> getNews()
        {
            var url = "http://feeds.feedburner.com/ign/news";
            using var reader = XmlReader.Create(url);
            var feed = SyndicationFeed.Load(reader);
            List<String> newsTitles = new List<String>();
            foreach (SyndicationItem item in feed.Items)
            {
                newsTitles.Add(item.Title.Text);
            }
            return newsTitles;
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
        [Authorize(Roles = "Admin,Developer")]
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
                                                List<IFormFile> newImages)
        {
            if (ModelState.IsValid)
            {
                GameImage gameImage;
                game.images = new List<GameImage>();

                // Saves all the new images
                foreach (IFormFile image in newImages)
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

                string currUserID = HttpContext.Session.GetString("userid"); ;
                var currDev = await _context.User
                    .Where(u => u.Id == currUserID)
                    .Select(u => u.developer)
                    .FirstOrDefaultAsync();

                game.developerId = currDev.id;
                game.developer = currDev;

                _context.Add(game);
                await _context.SaveChangesAsync();
                _twitterService.postTweet(game);
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
        [Authorize(Roles = "Admin,Developer")]
        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(string id)
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

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,
                                              [Bind("id,name,description,price")] Game game,
                                              List<IFormFile> newImages,
                                              List<string> imagesToDelete)
        {
            if (id != game.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Updates game data
                    _context.Update(game);

                    // Deletes the chosen images
                    _context.GameImage.Where((gi) => imagesToDelete.Contains(gi.id)).ToList()
                        .ForEach(gi => _context.GameImage.Remove(gi));

                    // Adds the new images
                    GameImage gameImage;
                    foreach (IFormFile image in newImages)
                    {
                        using (var ms = new MemoryStream())
                        {
                            image.CopyTo(ms);
                            byte[] fileBytes = ms.ToArray();

                            gameImage = new GameImage();
                            gameImage.fileBase64 = Convert.ToBase64String(fileBytes);
                            gameImage.fileContentType = image.ContentType;
                            gameImage.game = game;
                            _context.Add(gameImage);
                        }
                    }

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

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string query, List<String> genres, List<String> developers)
        {
            var searchResult = _context.Game
                .Include(g => g.generes)
                .Include(g => g.developer)
                .Where(g => ( query != null && query != "" ) ? g.name.Contains(query) : true)
                .Where(g => ( genres != null && genres.Count != 0 ) ? g.generes.Any(gg => genres.Contains(gg.id)) : true)
                .Where(g => ( developers != null && developers.Count != 0 ) ? developers.Contains(g.developer.id) : true)
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
