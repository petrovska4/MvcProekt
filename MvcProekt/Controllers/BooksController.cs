using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcProekt.Data;
using MvcProekt.Models;
using MvcProekt.ViewModel;
using MvcProekt.ViewModels;

namespace MvcProekt.Controllers
{
    public class BooksController : Controller
    {
        private readonly MvcProektContext _context;

        public BooksController(MvcProektContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string bookGenre, string searchString, string authorSearchString)
        {
            var genreQuery = _context.Genres
               .OrderBy(g => g.GenreName)
               .Select(g => g.GenreName)
               .Distinct();

            var booksQuery = _context.Books
                .Include(b => b.Reviews)
                .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
                .Include(b => b.Author)
                .AsQueryable();

            var authorsQuery = _context.Author.AsQueryable();
            
            if (!string.IsNullOrEmpty(bookGenre))
            {
                booksQuery = booksQuery.Where(b => b.BookGenres.Any(bg => bg.Genre.GenreName == bookGenre));
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                booksQuery = booksQuery.Where(b => b.Title.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(authorSearchString)) 
            {
                booksQuery = booksQuery.Where(a => (a.Author.FirstName + " " + a.Author.LastName).Contains(authorSearchString));
            }

            var genres = await genreQuery.ToListAsync();
            var books = await booksQuery.ToListAsync();
            var authors = await authorsQuery.ToListAsync();

            var viewModel = new BookGenreViewModel
            {
                Books = books,
                Genres = new SelectList(genres),
                BookGenre = bookGenre,
                SearchString = searchString,
                Authors = authors,
                AuthorSearchString = authorSearchString
            };

            return View(viewModel);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .Include(books => books.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

        // GET: Books/
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var genres = _context.Genres.OrderBy(g => g.GenreName).ToList();

            var viewModel = new BookGenresCreateViewModel
            {
                Book = new Books(),
                AuthorsList = _context.Author
                    .OrderBy(a => a.FirstName)
                    .Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.FullName,
                    }).ToList(),
                GenreList = new SelectList(genres, "Id", "GenreName")
            };

            return View(viewModel);
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(BookGenresCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewModel.Book);
                    await _context.SaveChangesAsync();
                    
                    foreach( int genreId in viewModel.SelectedGenres)
                    {
                        _context.BookGenre.Add(new BookGenre { GenreId = genreId, BookId = viewModel.Book.Id });
                    }
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while saving the entity changes.");
                    Console.WriteLine(ex.InnerException?.Message);
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .Where(m => m.Id == id)
                .Include(m => m.BookGenres)
                .FirstOrDefaultAsync();
            
            if (books == null)
            {
                return NotFound();
            }

            var genres = _context.Genres.AsEnumerable();
            genres = genres.OrderBy(s => s.GenreName);

            BookGenresEditViewModel viewModel = new BookGenresEditViewModel
            {
                Book = books,
                GenreList = new MultiSelectList(genres, "Id", "GenreName"),
                SelectedGenres = books.BookGenres.Select(sa => sa.GenreId).ToList()
            };

            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FullName", books.AuthorId);
            return View(viewModel);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, BookGenresEditViewModel viewModel)
        {
            if (id != viewModel?.Book?.Id)
            {
                return NotFound();
            }

            /*if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                var genres = _context.Genres.OrderBy(s => s.GenreName).AsEnumerable();
                viewModel.GenreList = new MultiSelectList(genres, "Id", "GenreName");

                ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FullName", viewModel.Book.AuthorId);
            }*/

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewModel.Book);
                    await _context.SaveChangesAsync();

                    IEnumerable<int> newGenreList = viewModel.SelectedGenres ?? new List<int>();
                    IEnumerable<int> prevGenreList = _context.BookGenre.Where(s => s.BookId == id).Select(s => s.GenreId).ToList();
                    
                    IQueryable<BookGenre> toBeRemoved = _context.BookGenre.Where(s => s.BookId == id);
                    if (newGenreList != null)
                    {
                        toBeRemoved = toBeRemoved.Where(s => !newGenreList.Contains(s.GenreId));
                        foreach (int actorId in newGenreList)
                        {
                            if (!prevGenreList.Any(s => s == actorId))
                            {
                                _context.BookGenre.Add(new BookGenre { GenreId = actorId, BookId = id });
                            }
                        }
                    }

                    _context.BookGenre.RemoveRange(toBeRemoved);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BooksExists(viewModel.Book.Id))
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

            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FullName", viewModel.Book.AuthorId);
            return View(viewModel);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Reviews)
                .Include(b => b.BookGenres)
                .ThenInclude(b => b.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'BookStoreContext.Books'  is null.");
            }
            var books = await _context.Books.FindAsync(id);
            if (books != null)
            {
                _context.Books.Remove(books);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BooksExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
