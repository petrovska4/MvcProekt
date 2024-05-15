using Microsoft.AspNetCore.Mvc.Rendering;
using MvcProekt.Models;

namespace MvcProekt.ViewModel

{
    public class BookGenreViewModel
    {
        public IList<Books> Books { get; set; }
        public SelectList Genres { get; set; }
        public string BookGenre { get; set; }
        public string SearchString { get; set; }
        public IList<Author> Authors { get; set; }
        public string AuthorSearchString { get; set; }
        public int Reviews { get; set; }
    }
}
