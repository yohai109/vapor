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
using vapor.services;

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
            var vaporContext = _context.Order
                .Include(o => o.customer)
                .Include(o => o.game)
                .ThenInclude(g => g.developer);
            return View(await vaporContext.ToListAsync());
        }

        public IActionResult Payment()
        {
            return View();
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

        [HttpGet]
        public async Task<IActionResult> Create(string gameid)
        {
            var cart = new List<String>();
            if (HttpContext.Session.Keys.Contains("cart"))
            {
                cart.AddRange(HttpContext.Session.GetListOfString("cart"));
            }

            if (gameid != null && gameid != "")
            {
                cart.Add(gameid);
            }

            var game = _context.Game
                .Where(g => cart.Contains(g.id))
                .Select(g => new Game
                {
                    id = g.id,
                    name = g.name,
                    developer = g.developer,
                    images = (ICollection<GameImage>)g.images
                        .Select(i => new GameImage { id = i.id })
                        .Take(1),
                    price = g.price
                })
                .ToListAsync();
            AddToCart(gameid);
            return View(await game);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Order()
        {
            string currUserID = HttpContext.Session.GetString("userid"); ;
            var currCustumer = await _context.User
                .Where(u => u.Id == currUserID)
                .Select(u => u.customer)
                .FirstOrDefaultAsync();

            var cart = HttpContext.Session.GetListOfString("cart");

            foreach (var gameid in cart)
            {
                var order = new Order
                {
                    gameId = gameid,
                    customerId = currCustumer.id,
                    date = DateTime.Now
                };
                _context.Add(order);
            }

            await _context.SaveChangesAsync();

            HttpContext.Session.SetListOfString("cart", new List<String>());
            return Json(new { });
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public IActionResult AddToCart(string gameid)
        {
            var cart = new List<String>();
            if (HttpContext.Session.Keys.Contains("cart"))
            {
                foreach (var currid in HttpContext.Session.GetListOfString("cart"))
                {
                    if (!cart.Contains(currid) && currid != "")
                    {
                        cart.Add(currid);
                    }
                }
            }

            if (!cart.Contains(gameid))
            {
                cart.Add(gameid);
            }
            HttpContext.Session.SetListOfString("cart", cart);

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
        public async Task<IActionResult> Delete(String gameid, String customerid)
        {
            if (gameid == null)
            {
                return NotFound();
            }

            if (customerid == null)
            {
                return NotFound();
            }
            var order = await _context.Order.Where(o => o.customerId == customerid).Where(o => o.gameId == gameid).ToListAsync();
            return View(order.FirstOrDefault());
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Order order)
        {
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(string id)
        {
            return _context.Order.Any(e => e.customerId == id);
        }
        /*public async Task<IActionResult> ConvertUserIdTo*/
    }
}
