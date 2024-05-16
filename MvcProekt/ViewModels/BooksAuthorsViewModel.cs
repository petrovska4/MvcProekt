using Microsoft.AspNetCore.Mvc.Rendering;
using MvcProekt.Models;
using System.Collections.Generic;

namespace MvcProekt.ViewModels
{
    public class BooksAuthorsViewModel
    {
        public string SearchName { get; set; }
        public string SearchSurname { get; set; }
        public IList<Author> Authors { get; set; }
    }
}
