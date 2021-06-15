using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using vapor.Data;
using vapor.Models;
using Microsoft.AspNetCore.Authorization;

namespace vapor.Controllers
{
    public class MapCoordinatesController : Controller
    {
        private readonly vaporContext _context;

        public MapCoordinatesController(vaporContext context)
        {
            _context = context;
        }

        // GET: MapCoordinates
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.MapCoordinates.ToListAsync());
        }

        public async Task<IActionResult> All()
        {
            return Json(await _context.MapCoordinates.ToListAsync());
        }

        // GET: MapCoordinates/Details/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mapCoordinates = await _context.MapCoordinates
                .FirstOrDefaultAsync(m => m.id == id);
            if (mapCoordinates == null)
            {
                return NotFound();
            }

            return View(mapCoordinates);
        }

        // GET: MapCoordinates/Create
        [Authorize(Roles = "Admin")]

        public IActionResult Create()
        {
            return View();
        }

        // POST: MapCoordinates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create([Bind("id,name,latitude,longitude")] MapCoordinates mapCoordinates)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mapCoordinates);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mapCoordinates);
        }

        // GET: MapCoordinates/Edit/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mapCoordinates = await _context.MapCoordinates.FindAsync(id);
            if (mapCoordinates == null)
            {
                return NotFound();
            }
            return View(mapCoordinates);
        }

        // POST: MapCoordinates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(string id, [Bind("id,name,latitude,longitude")] MapCoordinates mapCoordinates)
        {
            if (id != mapCoordinates.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mapCoordinates);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MapCoordinatesExists(mapCoordinates.id))
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
            return View(mapCoordinates);
        }

        // GET: MapCoordinates/Delete/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mapCoordinates = await _context.MapCoordinates
                .FirstOrDefaultAsync(m => m.id == id);
            if (mapCoordinates == null)
            {
                return NotFound();
            }

            return View(mapCoordinates);
        }

        // POST: MapCoordinates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var mapCoordinates = await _context.MapCoordinates.FindAsync(id);
            _context.MapCoordinates.Remove(mapCoordinates);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MapCoordinatesExists(string id)
        {
            return _context.MapCoordinates.Any(e => e.id == id);
        }
    }
}
