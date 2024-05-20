using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcProekt.Areas.Identity.Data;
using MvcProekt.Data;
using MvcProekt.Models;

namespace MvcProekt.Controllers
{
    public class UserBooksController : Controller
    {
        private readonly MvcProektContext _context;
        private readonly UserManager<MvcProektUser> _userManager;

        public UserBooksController(MvcProektContext context, UserManager<MvcProektUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: UserBooks
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var name = user.Email;

            var userBooks = await _context.UserBooks
                .Include(u => u.Book)
                    .ThenInclude(b => b.BookGenres)
                        .ThenInclude(bg => bg.Genre)
                .Include(u => u.Book)
                    .ThenInclude(b => b.Author)
                .Include(u => u.Book)
                    .ThenInclude(b => b.Reviews)
                .Where(ub => ub.AppUser == name)
                .ToListAsync();

            return View(userBooks);
        }

        // GET: UserBooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var name = user.Email;

            var userBooks = await _context.UserBooks
                .Include(u => u.Book)
                    .ThenInclude(b => b.BookGenres)
                        .ThenInclude(bg => bg.Genre)
                .Include(u => u.Book)
                    .ThenInclude(b => b.Author)
                .Include(u => u.Book)
                    .ThenInclude(b => b.Reviews)
                .Where(ub => ub.AppUser == name)
                .FirstOrDefaultAsync(b => b.Book.Id == id);

            return View(userBooks);
        }

        // GET: UserBooks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppUser,BookId")] UserBooks userBooks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userBooks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userBooks);
        }

        // GET: UserBooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBooks = await _context.UserBooks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userBooks == null)
            {
                return NotFound();
            }

            return View(userBooks);
        }

        // POST: UserBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userBooks = await _context.UserBooks.FindAsync(id);
            if (userBooks != null)
            {
                _context.UserBooks.Remove(userBooks);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserBooksExists(int id)
        {
            return _context.UserBooks.Any(e => e.Id == id);
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(int id, string input, int rating)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var name = user.Email;

            var book = await _context.Books.FirstOrDefaultAsync(e => e.Id == id);

            var review = new Review
            {
                BookId = book.Id,
                AppUser = name,
                Comment = input,
                Rating = rating,
                Book = book
            };

            _context.Review.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
