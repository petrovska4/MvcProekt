using Microsoft.AspNetCore.Mvc.Rendering;
using MvcProekt.Models;
using System.Collections.Generic;

namespace MvcProekt.ViewModels
{
    public class BooksAuthorsEditViewModel
    {
        public Books? Books { get; set; }
        public IEnumerable<int>? SelectedAuthors { get; set; }
        public IEnumerable<SelectListItem>? AuthorList { get; set; }
    }
}
