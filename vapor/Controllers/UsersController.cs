using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using vapor.Data;
using vapor.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using vapor.services;

namespace vapor.Controllers
{
    //[AllowAnonymous]
    public class UsersController : Controller
    {
        private readonly vaporContext _context;
        private twitter _twitterService;
        public UsersController(vaporContext context, twitter twitterService)
        {
            _context = context;
            _twitterService = twitterService;
        }

        public async Task<IActionResult> Logout()
        {
            //HttpContext.Session.Clear();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        // POST: Users/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Id,Username,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var q = from u in _context.User
                        where u.Username == user.Username && u.Password == user.Password
                        select u;

                // var q = _context.User.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);

                if (q.Count() > 0)
                {
                    //HttpContext.Session.SetString("username", q.First().Username);

                    Signin(q.First());
                    return RedirectToAction(nameof(Index), "Games");
                }
                else
                {
                    ViewData["Error"] = "Username and/or password are incorrect.";
                }
            }
            return View(user);
        }

        private async void Signin(User account)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.Username),
                    new Claim(ClaimTypes.Role, account.Type.ToString()),
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            };
            HttpContext.Session.SetString("username", account.Username);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        // GET: Users/Register
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult RegisterDeveloper()
        {
            return View();
        }

        // POST: Users/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        /*        [ValidateAntiForgeryToken]*/
        public async Task<IActionResult> Register(String Id, String Username, String Password, String name, String email, String firstName, String lastName, String phoneNumber, int type, IFormFile developerAvater)
        {
            if(type == 1)
            {

                var new_developer = new Developer
                {
                    name = Username
                };
                using (var ms = new MemoryStream())
                {
                    developerAvater.CopyTo(ms);
                    byte[] fileBytes = ms.ToArray();
                    new_developer.avatar = Convert.ToBase64String(fileBytes);
                    new_developer.fileContentType = developerAvater.ContentType;
                    
                }
                _context.Add(new_developer);
                await _context.SaveChangesAsync();
                _twitterService.postTweet(new_developer);
                var complete_user = await _context.Developer.Where(u => u.name == new_developer.name).FirstAsync();
                var new_user = new User
                {
                    Username = Username,
                    Password = Password,
                    developerID = complete_user.id,
                    Type = UserType.Developer

                };
                _context.Add(new_user);
                await _context.SaveChangesAsync();

                Signin(new_user);

                return View(new_user);
            }
            if (type == 0)
            {
                var new_customer = new Customer
                {
                    email = email,
                    firstName = firstName,
                    lastName = lastName,
                    phoneNumber = phoneNumber,
                    name = Username
                };
                _context.Add(new_customer);
                await _context.SaveChangesAsync();

                var complete_user = await _context.Customer.Where(u => u.name == new_customer.name).FirstAsync();
                var new_user = new User
                {
                    Username = Username,
                    Password = Password,
                    customerID = complete_user.id,
                    Type = UserType.Customer

                };
                _context.Add(new_user);
                await _context.SaveChangesAsync();

                Signin(new_user);

                return View(new_user);
            }
            return View();


        }

        [Authorize(Roles = "Admin")]
        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(String? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [Authorize(Roles = "Admin")]
        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,name,email,firstName,lastName,phoneNumber")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        [Authorize(Roles = "Admin")]
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(String id, [Bind("Id,Username,Password,Type")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }
        [Authorize(Roles = "Admin")]
        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(String? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [Authorize(Roles = "Admin")]
        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(String id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        private bool UserExists(String id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
