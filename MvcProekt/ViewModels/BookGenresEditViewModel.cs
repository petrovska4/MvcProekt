using Microsoft.AspNetCore.Mvc.Rendering;
using MvcProekt.Models;
using System.ComponentModel.DataAnnotations;

namespace MvcProekt.ViewModels
{
    public class BookGenresEditViewModel
    {
        public Books Book { get; set; }
        public IEnumerable<SelectListItem>? GenreList { get; set; }
        public IEnumerable<int>? SelectedGenres { get; set; }
    }
}
