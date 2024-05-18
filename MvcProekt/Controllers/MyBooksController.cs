using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcProekt.Data;
using MvcProekt.Models;

namespace MvcProekt.Controllers
{
    public class MyBooksController : Controller
    {
        private readonly MvcProektContext _context;

        public MyBooksController(MvcProektContext context)
        {
            _context = context;
        }

        // GET: MyBooks
        public async Task<IActionResult> Index()
        {
            return View(await _context.MyBooks.ToListAsync());
        }

        // GET: MyBooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myBooks = await _context.MyBooks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (myBooks == null)
            {
                return NotFound();
            }

            return View(myBooks);
        }

        // GET: MyBooks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MyBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] MyBooks myBooks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(myBooks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(myBooks);
        }

        // GET: MyBooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myBooks = await _context.MyBooks.FindAsync(id);
            if (myBooks == null)
            {
                return NotFound();
            }
            return View(myBooks);
        }

        // POST: MyBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] MyBooks myBooks)
        {
            if (id != myBooks.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(myBooks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MyBooksExists(myBooks.Id))
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
            return View(myBooks);
        }

        // GET: MyBooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myBooks = await _context.MyBooks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (myBooks == null)
            {
                return NotFound();
            }

            return View(myBooks);
        }

        // POST: MyBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var myBooks = await _context.MyBooks.FindAsync(id);
            if (myBooks != null)
            {
                _context.MyBooks.Remove(myBooks);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MyBooksExists(int id)
        {
            return _context.MyBooks.Any(e => e.Id == id);
        }
    }
}
