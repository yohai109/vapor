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
    public class DevelopersController : Controller
    {
        private readonly vaporContext _context;

        public DevelopersController(vaporContext context)
        {
            _context = context;
        }

        // GET: Developers
        public async Task<IActionResult> Index()
        {
            return View(/*await _context.Developer.ToListAsync()*/);
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

        // GET: Developers/Details/5
        public async Task<IActionResult> Details(string id)
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

        // GET: Developers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Developers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                return RedirectToAction(nameof(Index));
            }
            return View(developer);
        }

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


       // [HttpGet]
        //public Task<ActionResult> GetDeveloperImage(string avatar, string fileContentType)
        //{
         //   if (avatar == null || fileContentType == null)
          //  {
         //       return NotFound();
        /*    }

            byte[] fileBytes = Convert.FromBase64String(avatar);
            return this.File(fileBytes, fileContentType);
        }*/

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

        private bool DeveloperExists(string id)
        {
            return _context.Developer.Any(e => e.id == id);
        }
    }
}
