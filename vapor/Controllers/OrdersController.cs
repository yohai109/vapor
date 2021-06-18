using System;
using System.Collections.Generic;
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
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly vaporContext _context;

        public OrdersController(vaporContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var vaporContext = _context.Order.Include(o => o.customer).Include(o => o.game);
            return View(await vaporContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.customer)
                .Include(o => o.game)
                .FirstOrDefaultAsync(m => m.customerId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        [HttpGet]
        public async Task<IActionResult> Create(string gameid)
        {
            return View(await _context.Game.Where(g => g.id == gameid).FirstOrDefaultAsync());
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Order(string gameid)
        {
            string currUserID = HttpContext.Session.GetString("userid"); ;
            var currCustumer = await _context.User
                .Where(u => u.Id == currUserID)
                .Select(u => u.customer)
                .FirstOrDefaultAsync();

            var order = new Order
            {
                gameId = gameid,
                customerId = currCustumer.id,
                date = DateTime.Now
            };

            _context.Add(order);
            await _context.SaveChangesAsync();
            return Json(new { });
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["customerId"] = new SelectList(_context.Customer, "id", "id", order.customerId);
            ViewData["gameId"] = new SelectList(_context.Game, "id", "id", order.gameId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("gameId,customerId,date")] Order order)
        {
            if (id != order.customerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.customerId))
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
            ViewData["customerId"] = new SelectList(_context.Customer, "id", "id", order.customerId);
            ViewData["gameId"] = new SelectList(_context.Game, "id", "id", order.gameId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.customer)
                .Include(o => o.game)
                .FirstOrDefaultAsync(m => m.customerId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(string id)
        {
            return _context.Order.Any(e => e.customerId == id);
        }
    }
}
