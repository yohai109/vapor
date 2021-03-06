using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using vapor.Data;
using vapor.Models;

namespace vapor.Controllers
{

    public class ReviewsController : Controller
    {
        private readonly vaporContext _context;

        public ReviewsController(vaporContext context)
        {
            _context = context;
        }

        // GET: Reviews
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {

            var vaporContext = _context.Review
                .Include(g => g.game)
                .Include(c => c.cusotmer);
            return View(await vaporContext.ToListAsync());

           /* return View(await _context.Review.ToListAsync());*/
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .FirstOrDefaultAsync(m => m.id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,rating,comment,writtenAt,lastUpdate")] Review review)
        {
            if (ModelState.IsValid)
            {
                review.writtenAt = DateTime.Now;
                review.lastUpdate = DateTime.Now;
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("id,rating,comment,writtenAt,lastUpdate,gameId,customerId,game,cusotmer")] Review review)
        {
            if (id != review.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    review.lastUpdate = DateTime.Now;
                    //review.writtenAt = _context.Entry(review). fix written time 0 bug
                    review.cusotmer = await _context.Customer.FindAsync(review.customerId);
                    review.game = await _context.Game.FindAsync(review.gameId);
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.id))
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
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .FirstOrDefaultAsync(m => m.id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }


        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var review = await _context.Review.FindAsync(id);
            _context.Review.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(string id)
        {
            return _context.Review.Any(e => e.id == id);
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ReviewUserName(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            Customer customer = await _context.Customer.FirstAsync(c => c.id == id);


            if (customer == null)
            {
                return NotFound();
            }
            string username = customer.name;

            return Json(new { username = username });
        }
    }
}
