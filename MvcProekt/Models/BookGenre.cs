namespace MvcProekt.Models
{
    public class BookGenre
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int GenreId { get; set; }
        public Books? Book { get; set; }
        public Genres? Genre { get; set; }
    }
}
