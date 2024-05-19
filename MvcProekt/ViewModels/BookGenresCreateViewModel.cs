using MvcProekt.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcProekt.ViewModel
{
    public class BookGenresCreateViewModel
    {
        public Books? Book { get; set; }
        public IEnumerable<int>? SelectedGenres { get; set; }
        public IEnumerable<SelectListItem>? GenreList { get; set; }
        public IEnumerable<SelectListItem>? AuthorsList { get; set; }
        public IFormFile? FrontPageFile { get; set; }
        public IFormFile? PdfFile { get; set; }
    }
}
