using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MvcProekt.Areas.Identity.Data;
using MvcProekt.Data;
using MvcProekt.Models;
using MvcProekt.ViewModel;
using MvcProekt.ViewModels;

namespace MvcProekt.Controllers
{
    public class BooksController : Controller
    {
        private readonly MvcProektContext _context;
        private readonly UserManager<MvcProektUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public BooksController(MvcProektContext context, UserManager<MvcProektUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
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

        public async Task<IActionResult> IndexById(int id)
        {
            //knigata shto treba da se dodade
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .Include(books => books.Reviews)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
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
                .Where(ub => ub.AppUser == name)
                .ToListAsync();

            var exist = userBooks.Any(b => b.Book.Id == id);

            if (exist)
            {
                return RedirectToAction(nameof(Index));
            }

            var newMyBook = new UserBooks
            {
                BookId = id,
                AppUser = name
            };

            _context.UserBooks.Add(newMyBook);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

            return RedirectToAction(nameof(Index));
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
                if (viewModel.FrontPageFile != null && viewModel.FrontPageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.FrontPageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.FrontPageFile.CopyToAsync(fileStream);
                    }

                    // Save file path in the database
                    viewModel.Book.FrontPage = "/images/" + uniqueFileName;
                    //book.FrontPage = filePath;
                }
                if (viewModel.PdfFile != null && viewModel.PdfFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                    string uniquePdfFileName = Guid.NewGuid().ToString() + "_" + viewModel.PdfFile.FileName;
                    string pdfFilePath = Path.Combine(uploadsFolder, uniquePdfFileName);

                    using (var pdfFileStream = new FileStream(pdfFilePath, FileMode.Create))
                    {
                        await viewModel.PdfFile.CopyToAsync(pdfFileStream);
                    }

                    // Save PDF file path in the database
                    viewModel.Book.DownloadUrl = "/images/" + uniquePdfFileName;
                    //book.DownloadURL = pdfFilePath;
                }

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
                    if (viewModel.FrontPageFile != null && viewModel.FrontPageFile.Length > 0)
                    {
                        // Save FrontPageFile
                        string uniqueFrontPageFileName = Guid.NewGuid().ToString() + "_" + viewModel.FrontPageFile.FileName;
                        string frontPageFilePath = Path.Combine(_environment.WebRootPath, "images", uniqueFrontPageFileName);

                        using (var fileStream = new FileStream(frontPageFilePath, FileMode.Create))
                        {
                            await viewModel.FrontPageFile.CopyToAsync(fileStream);
                        }

                        viewModel.Book.FrontPage = "/images/" + uniqueFrontPageFileName; // Update file path
                    }

                    // Check if PdfFile is uploaded
                    if (viewModel.PdfFile != null && viewModel.PdfFile.Length > 0)
                    {
                        // Save PdfFile
                        string uniquePdfFileName = Guid.NewGuid().ToString() + "_" + viewModel.PdfFile.FileName;
                        string pdfFilePath = Path.Combine(_environment.WebRootPath, "images", uniquePdfFileName);

                        using (var fileStream = new FileStream(pdfFilePath, FileMode.Create))
                        {
                            await viewModel.PdfFile.CopyToAsync(fileStream);
                        }

                        viewModel.Book.DownloadUrl = "/images/" + uniquePdfFileName; // Update file path
                    }

                    // If FrontPageFile and PdfFile are not uploaded, retain the existing values
                    if (viewModel.FrontPageFile == null && viewModel.PdfFile == null)
                    {
                        var existingBook = _context.Books.AsNoTracking().FirstOrDefault(b => b.Id == id);
                        if (existingBook != null)
                        {
                            viewModel.Book.FrontPage = existingBook.FrontPage;
                            viewModel.Book.DownloadUrl = existingBook.DownloadUrl;
                        }
                    }

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

        public IActionResult ViewFile(string fileName)
        {
            var filePath = Path.Combine(_environment.WebRootPath, "images", fileName);
            if (System.IO.File.Exists(filePath))
            {
                var provider = new FileExtensionContentTypeProvider();
                if (provider.TryGetContentType(fileName, out var contentType))
                {
                    return PhysicalFile(filePath, contentType);
                }
                else
                {
                    return PhysicalFile(filePath, "application/octet-stream"); // Default to octet-stream if MIME type cannot be determined
                }
            }
            else
            {
                return NotFound(); // Return 404 if the file does not exist
            }
        }
    }

}
