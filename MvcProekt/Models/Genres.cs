using System.ComponentModel.DataAnnotations;

namespace MvcProekt.Models
{
    public class Genres
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string? GenreName { get; set; }
        public ICollection<BookGenre>? BookGenres { get; set; }
    }
}
