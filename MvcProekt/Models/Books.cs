using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcProekt.Models
{
    public class Books
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string? Title { get; set; }
        [Display(Name = "Year Published")]
        public int YearPublished { get; set; }
        [Display(Name = "Number od pages")]
        public int NumPages { get; set; }
        public string? Description { get; set; }
        [StringLength(50)]
        public string? Publisher { get; set; }
        [Display(Name = "Front Page")]
        public string? FrontPage { get; set; }
        [Display(Name = "Download here")]
        public string? DownloadUrl { get; set;}
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
        public ICollection<BookGenre>? BookGenres { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<UserBooks>? UserBooks { get; set; }
        public double AverageRating()
        {
            if (Reviews != null && Reviews.Any())
            {
                int totalRating = (int)Reviews.Sum(r => r.Rating);
                double average = (double)totalRating / Reviews.Count;
                return Math.Round(average, 1);
            }

            return 0;
        }
    }
}
