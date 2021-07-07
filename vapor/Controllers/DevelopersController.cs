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
using Microsoft.AspNetCore.Authorization;
using vapor.services;

namespace vapor.Controllers
{

    public class DevelopersController : Controller
    {
        private readonly vaporContext _context;
        private twitter _twitterService;

        public DevelopersController(vaporContext context, twitter twitterService)
        {
            _context = context;
            _twitterService = twitterService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Developer.ToListAsync());
        }

        public async Task<IActionResult> All()
        {
            /*byte[] fileBytes = Convert.FromBase64String(gameImage.fileBase64);
            return this.File(fileBytes, gameImage.fileContentType);
*/

            return Json(await _context.Developer.ToListAsync());
        }
        public async Task<IActionResult> Id(String id)
        {
            /*byte[] fileBytes = Convert.FromBase64String(gameImage.fileBase64);
            return this.File(fileBytes, gameImage.fileContentType);
*/
            if (id == null)
            {
                return NotFound();
            }

            var developer = await _context.Developer
                .FirstOrDefaultAsync(m => m.id == id);
            if (developer == null)
            {
                return NotFound();
            }

            return Json(await _context.Developer.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> GetDeveloperImage(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Developer developer = await _context.Developer.FirstOrDefaultAsync(d => d.id == id);

            if (developer == null)
            {
                return NotFound();
            }

            byte[] fileBytes = Convert.FromBase64String(developer.avatar);
            return this.File(fileBytes, developer.fileContentType);
        }

        //[AllowAnonymous][Authorize(Roles = "Admin,Developer")]
        [AllowAnonymous]
        // GET: Developers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var developer = await _context.Developer
                .Where(d => d.id == id)
                .Include(d => d.games).ThenInclude(g => g.reviews)
                .Include(d => d.games).ThenInclude(g => g.genres)
                .FirstOrDefaultAsync(m => m.id == id);
            if (developer == null)
            {
                return NotFound();
            }

            return View(developer);
        }

        [Authorize(Roles = "Admin")]
        // GET: Developers/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        // POST: Developers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,avatar")] Developer developer, IFormFile developerAvater)
        {
            if (ModelState.IsValid)
            {
                using (var ms = new MemoryStream())
                {
                    developerAvater.CopyTo(ms);
                    byte[] fileBytes = ms.ToArray();
                    developer.avatar = Convert.ToBase64String(fileBytes);
                    developer.fileContentType = developerAvater.ContentType;
                }
                _context.Add(developer);
                await _context.SaveChangesAsync();
                _twitterService.postTweet(developer);
                return RedirectToAction(nameof(Index));
            }


            return View(developer);
        }

        [Authorize(Roles = "Admin")]
        // GET: Developers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var developer = await _context.Developer.FindAsync(id);
            if (developer == null)
            {
                return NotFound();
            }
            return View(developer);
        }

        [Authorize(Roles = "Admin")]
        // POST: Developers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("id,name,avatar")] Developer developer, IFormFile developerAvater)
        {
            if (id != developer.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        developerAvater.CopyTo(ms);
                        byte[] fileBytes = ms.ToArray();
                        developer.avatar = Convert.ToBase64String(fileBytes);
                        developer.fileContentType = developerAvater.ContentType;
                    }
                    _context.Update(developer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeveloperExists(developer.id))
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
            return View(developer);
        }
        [Authorize(Roles = "Admin")]
        // GET: Developers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var developer = await _context.Developer
                .FirstOrDefaultAsync(m => m.id == id);
            if (developer == null)
            {
                return NotFound();
            }

            return View(developer);
        }
        [Authorize(Roles = "Admin")]
        // POST: Developers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var developer = await _context.Developer.FindAsync(id);
            _context.Developer.Remove(developer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string devName, string gameName, int numOfGames = 1)
        {
            List<Developer> result = new List<Developer>();

            if (numOfGames == 1)
            {
                if (gameName == null || gameName == "")
                {
                    var developerSeach = _context.Developer
                    .Where(d => ( devName != null && devName != "" ) ? d.name.Contains(devName) : true);

                    result.AddRange(await developerSeach.ToListAsync());
                }
                else
                {
                    var gameSearch = _context.Game
                            .Include(g => g.developer)
                            .Where(g => g.name.Contains(gameName))
                            .Where(g => ( devName != null && devName != "" ) ? g.developer.name.Contains(devName) : true)
                            .Select(d => d.developer);

                    result.AddRange(await gameSearch.ToListAsync());
                }
            }
            else if (numOfGames > 1)
            {
                var numofGameSearchIds = await _context.Game
                    .Include(g => g.developer)
                    .GroupBy(g => g.developerId)
                    .Select(gb => new
                    {
                        developerid = gb.Key,
                        count = gb.Count()
                    })
                    .Where(devId => devId.count >= numOfGames)
                    .Select(gb => gb.developerid)
                    .ToListAsync();

                var devs = _context.Game
                            .Include(g => g.developer)
                            .Where(g => g.name.Contains(gameName))
                            .Where(g => ( devName != null && devName != "" ) ? g.developer.name.Contains(devName) : true)
                            .Select(d => d.developer)
                            .Where(d => numofGameSearchIds.Contains(d.id));

                result.AddRange(await devs.ToListAsync());
            }

            return Json(result);
        }

        [HttpGet]
        public async Task<ActionResult> getDeveloperAvater(string devId)
        {
            if (devId == null)
            {
                return NotFound();
            }

            Developer developer = await _context.Developer.FirstOrDefaultAsync(d => d.id == devId);

            if (developer == null)
            {
                return NotFound();
            }

            byte[] fileBytes = Convert.FromBase64String(developer.avatar);
            return this.File(fileBytes, developer.fileContentType);
        }

        private bool DeveloperExists(string id)
        {
            return _context.Developer.Any(e => e.id == id);
        }
    }
}
