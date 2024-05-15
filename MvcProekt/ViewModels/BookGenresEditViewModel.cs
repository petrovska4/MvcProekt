using Microsoft.AspNetCore.Mvc.Rendering;
using MvcProekt.Models;

namespace MvcProekt.ViewModels
{
    public class BookGenresEditViewModel
    {
        public Books? Book { get; set; }
        public IEnumerable<int>? SelectedGenres { get; set; }
        public IEnumerable<SelectListItem>? GenreList { get; set; }
    }
}
