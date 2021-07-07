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
                .Include(g => g.genres)
                .Include(g => g.developer)
                .Include(g => g.images)
                .Select(g => new Game
                {
                    id = g.id,
                    name = g.name,
                    developer = g.developer,
                    genres = (ICollection<Genre>)g.genres.Take(4),
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
            dynamic model = new ExpandoObject();
            if (id == null)
            {
                return NotFound();
            }

            var loadedGame = await _context.Game
                .Include(g => g.genres)
                .Include(g => g.developer)
                .Select(game => new
                {
                    game,
                    game.developer,
                    images = game.images.Select(i => new GameImage { id = i.id }).ToList()
                })
                .FirstOrDefaultAsync(g => g.game.id == id);

            if (loadedGame == null)
            {
                return NotFound();
            }

            loadedGame.game.images = loadedGame.images;
            loadedGame.game.developer = loadedGame.developer;
            model.game = loadedGame.game;
            loadedGame.game.images.Count();
            string currUserID = HttpContext.Session.GetString("userid");

            var currCustomer = await _context.User
                    .Where(u => u.Id == currUserID)
                    .Select(u => u.customer)
                    .FirstOrDefaultAsync();

            var customerReview = await _context.Review
                .Where(r => r.cusotmer == currCustomer)
                .FirstOrDefaultAsync();

            model.currentCustomer = currCustomer;
            model.customerReview = customerReview;

            /*var otherReviews = await _context.Review
                .Where(r => r.cusotmer != currCustomer)
                .ToListAsync();*/

            var otherReviews = _context.Review
                .Where(r => r.gameId.Equals(id) && r.cusotmer != currCustomer)
                .OrderByDescending(r => r.lastUpdate)
                .Select(r => new Review
                {
                    id = r.id,
                    gameId = r.gameId,
                    comment = r.comment,
                    rating = r.rating,
                    writtenAt = r.writtenAt,
                    lastUpdate = r.lastUpdate,
                    cusotmer = new Customer
                    {
                        name = r.cusotmer.name
                    }
                });

            model.reviews = otherReviews;

            var currUserReview = _context.Review
                .Where(r => r.gameId.Equals(id) && r.cusotmer.Equals(currCustomer))
                .Select(r => new Review
                {
                    id = r.id,
                    gameId = r.gameId,
                    comment = r.comment,
                    rating = r.rating,
                    writtenAt = r.writtenAt,
                    lastUpdate = r.lastUpdate,
                    cusotmer = new Customer
                    {
                        name = r.cusotmer.name
                    }
                })
                .FirstOrDefaultAsync();

            model.currUserReview = await currUserReview;

            var currCustomerOrder = await _context.Order
            .Include(o => o.customer)
            .Where(o => o.customer == currCustomer && o.gameId==id)
            .FirstOrDefaultAsync();

            model.currCustomerOrder = currCustomerOrder;

            var avarageRate = await _context.Review
                .Where(r => r.gameId.Equals(id))
                .GroupBy(r => r.gameId)
                .Select(gb => gb.Average(r => r.rating))
                .FirstOrDefaultAsync();


            model.avarageRate = avarageRate;


            return View(model);
        }
        [Authorize(Roles = "Admin,Developer")]
        // GET: Games/Create
        public IActionResult Create()
        {
            ViewBag.genreSelectList = new SelectList(_context.Genre, nameof(Genre.id), nameof(Genre.name)).ToList();
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,price,name,description")] Game game,
                                                List<String> newGenreIds,
                                                List<IFormFile> newImages)
        {

            if (ModelState.IsValid)
            {
                GameImage gameImage;
                game.images = new List<GameImage>();
                game.releaseDate = DateTime.Now;

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

                // Connects between the game to the proper developer
                string currUserID = HttpContext.Session.GetString("userid"); ;
                var currDev = await _context.User
                    .Where(u => u.Id == currUserID)
                    .Select(u => u.developer)
                    .FirstOrDefaultAsync();

                game.developerId = currDev.id;
                game.developer = currDev;

                // Connects between the game to the game genres
                var genresList = await _context.Genre.Where(g => newGenreIds.Contains(g.id)).ToListAsync();
                game.genres = genresList;

                _context.Add(game);
                await _context.SaveChangesAsync();
                _twitterService.postTweet(game);
                return RedirectToAction(nameof(Index));
            }
            ViewData["developerId"] = new SelectList(_context.Developer, "id", "id", game.developerId);
            return View(game);
        }


        [HttpGet]
        public async Task<ActionResult> GetGameImage(string gameImageId)
        {
            if (gameImageId == null)
            {
                return NotFound();
            }

            GameImage gameImage = await _context.GameImage.FirstOrDefaultAsync(gi => gi.id == gameImageId);

            if (gameImage == null)
            {
                return NotFound();
            }

            byte[] fileBytes = Convert.FromBase64String(gameImage.fileBase64);
            return this.File(fileBytes, gameImage.fileContentType);
        }

        [HttpGet]
        public async Task<ActionResult> GetGameImageByGameId(string gameId)
        {
            if (gameId == null)
            {
                return NotFound();
            }

            GameImage gameImage = await _context.GameImage.FirstOrDefaultAsync(gi => gi.gameID == gameId);

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
                .Include(g => g.genres)
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
            var genreSelectList = new SelectList(_context.Genre, nameof(Genre.id), nameof(Genre.name)).ToList();

            List<String> relatedGenreIds = loadedGame.game.genres.Select(g => g.id).ToList();
            foreach (var item in genreSelectList)
            {
                if (relatedGenreIds.Contains(item.Value))
                {
                    item.Selected = true;
                }
            }
            ViewBag.genreSelectList = genreSelectList;
            return View(loadedGame.game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,
                                              [Bind("id,name,description,price")] Game game,
                                              List<String> updatedGenreIds,
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
                    Game updatedGame = await _context.Game
                        .Include(g => g.genres)
                        .FirstAsync(g => g.id == id);
                    updatedGame.name = game.name;
                    updatedGame.description = game.description;
                    updatedGame.price = game.price;

                    // Connects between the game to the game genres
                    var genresList = await _context.Genre.Where(g => updatedGenreIds.Contains(g.id)).ToListAsync();
                    updatedGame.genres.Clear();
                    foreach (Genre genre in genresList)
                    {
                        updatedGame.genres.Add(genre);
                    }
                    _context.Update(updatedGame);

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
                            gameImage.game = updatedGame;
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
              .Include(g => g.genres)
              .Include(g => g.developer)
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
            //delete all reviews of this game
            var reviews = await _context.Review.Where(r => r.gameId == id).ToListAsync();
            _context.Review.RemoveRange(reviews);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string query, List<String> genres, List<String> developers, float avgRating = 0)
        {
            var searchResult = _context.Game
                .Include(g => g.genres)
                .Include(g => g.developer)
                .Include(g => g.reviews)
                .Where(g => ( query != null && query != "" ) ? g.name.Contains(query) : true)
                .Where(g => ( genres != null && genres.Count != 0 ) ? g.genres.Any(gg => genres.Contains(gg.id)) : true)
                .Where(g => ( developers != null && developers.Count != 0 ) ? developers.Contains(g.developer.id) : true)
                .Where(g => g.reviews.Average(r => r.rating) >= avgRating)
                .Select(g => new
                {
                    id = g.id,
                    name = g.name,
                    developer = g.developer.name,
                    genres = g.genres,
                    imageid = g.images.FirstOrDefault().id
                });

            return Json(await searchResult.ToListAsync());
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Reviews(string gameId)
        {
            if (gameId == null)
            {
                return NotFound();
            }

            var reviews = _context.Review
                .Where(r => r.gameId.Equals(gameId))
                .OrderByDescending(r => r.lastUpdate)
                .Select(r => new Review
                {
                    gameId = r.gameId,
                    comment = r.comment,
                    rating = r.rating,
                    cusotmer = new Customer
                    {
                        name = r.cusotmer.name
                    }
                });

            if (reviews == null)
            {
                return NotFound();
            }

            return Json(await reviews.ToListAsync());
        }

        [HttpPut]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReview(string id, int rating, string comment)
        {
            Review updatedReview = await _context.Review.FirstAsync(r => r.id == id);
            /*            string username = "";
            */
            if (id != updatedReview.id)
            {
                return NotFound();
            }
            DateTime time = DateTime.Now;
            updatedReview.lastUpdate = time;
            updatedReview.rating = rating;
            updatedReview.comment = comment;



            //review.writtenAt = _context.Entry(review). fix written time 0 bug
            try
            {
                _context.Update(updatedReview);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            //return Ok(time.ToString("MM/dd/yyyy hh:mm:ss tt"));

            return Json(new { Review = updatedReview, time = time.ToString("MM/dd/yyyy hh:mm:ss tt") });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReview(int rating, string comment, string gameId)
        {
            string currUserID = HttpContext.Session.GetString("userid");

            var currCustomer = await _context.User
                    .Where(u => u.Id == currUserID)
                    .Select(u => u.customer)
                    .FirstOrDefaultAsync();
            string customerId = currCustomer.id;

            Review alreadyExistReview = await _context.Review
                .Where(r => r.customerId == customerId && r.gameId == gameId)
                .FirstOrDefaultAsync();
            if (alreadyExistReview != null)
            {
                return NotFound();
            }
            Review newReview = new Review();
            newReview.customerId = customerId;
            newReview.gameId = gameId;
            newReview.rating = rating;
            newReview.comment = comment;
            DateTime time = DateTime.Now;
            newReview.writtenAt = time;
            newReview.lastUpdate = time;

            try
            {
                _context.Add(newReview);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return NotFound();
            }
            //Response.WriteAsync("https://localhost:44334/Games/Details/a6129515-22e5-4c38-8f1e-0564291307c6");
            //return Ok(time.ToString("MM/dd/yyyy hh:mm:ss tt"));
            /*Task < IActionResult > task = Details(gameId);
            return await task;*/
            Review enteredReview = await _context.Review
                .Where(r => r.customerId == customerId && r.gameId == gameId)
                .FirstOrDefaultAsync();
            string username = enteredReview.cusotmer.name;
            enteredReview.cusotmer = null;


            return Json(new { Review = enteredReview, username = username, time = time.ToString("MM/dd/yyyy hh:mm:ss tt") });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReview(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Review review = await _context.Review
                .Where(r => r.id == id)
                .FirstOrDefaultAsync();


            if (review == null)
            {
                return NotFound();
            }

            try
            {
                _context.Review.Remove(review);
                await _context.SaveChangesAsync();

            }
            catch
            {
                return NotFound();
            }

            return Ok(true);
        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RatingAvarage(string gameId)
        {
            if (gameId == null)
            {
                return NotFound();
            }

            var avarageRate = _context.Review
                .Where(r => r.gameId.Equals(gameId))
                .GroupBy(r => r.gameId)
                .Select(gb => new
                {
                    avg = gb.Average(r => r.rating)
                });


            if (avarageRate == null)
            {
                return NotFound();
            }

            return Json(await avarageRate.ToListAsync());
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
