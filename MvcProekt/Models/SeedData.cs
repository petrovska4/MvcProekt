using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcProekt.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace MvcProekt.Models
{ 
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MvcProektContext(
            serviceProvider.GetRequiredService<
            DbContextOptions<MvcProektContext>>()))
            {
                // Look for any movies.
                if (context.Books.Any() || context.Author.Any() || context.Genres.Any() || context.UserBooks.Any() || context.BookGenre.Any())
                {
                    return; // DB has been seeded
                }
                context.Books.AddRange(
                    new Books 
                    { 
                        Title = "War and Peace", 
                        YearPublished = 1986, 
                        NumPages = 1200,  
                        Description = "\"War and Peace\" is an epic novel by Leo Tolstoy that chronicles the lives of Russian aristocrats against the backdrop of Napoleon's invasion of Russia, exploring themes of love, war, politics, and the human condition.", 
                        Publisher = "Russkiy Vestnik", 
                        AuthorId = 8, 
                        DownloadUrl = "https://www.planetebook.com/free-ebooks/war-and-peace.pdf", 
                        FrontPage = "https://www.booksoftitans.com/wp-content/uploads/2017/12/war-and-peace.jpg"
                    },
                    new Books
                    {
                        Title = "Crime and Punishment",
                        YearPublished = 1866,
                        NumPages = 500,
                        Description = "\"Crime and Punishment\" follows the psychological turmoil of a young Russian intellectual, Raskolnikov, who commits a murder and grapples with the moral consequences, exploring themes of guilt, redemption, and the human condition in 19th-century St. Petersburg.",
                        Publisher = "Russkiy Vestnik",
                        AuthorId = 9,
                        DownloadUrl = "https://www.planetebook.com/free-ebooks/crime-and-punishment.pdf",
                        FrontPage = "https://qph.cf2.quoracdn.net/main-qimg-60ca2932975ad8fac18089434f1c0d3f-lq"
                    }
                );
                context.SaveChanges();

                context.Author.AddRange(
                    new Author
                    {
                        FirstName = "Leo",
                        LastName = "Tolstoy",
                        Date = DateOnly.Parse("1828-9-09"),
                        Nationality = "Russian",
                        Gender = "Male"
                    },
                    new Author 
                    { 
                        FirstName = "Fyodor", 
                        LastName = "Dostoevsky", 
                        Date = DateOnly.Parse("1821-11-11"), 
                        Nationality = "Russian", 
                        Gender = "Male" 
                    }
                );
                context.SaveChanges();

                context.Genres.AddRange(
                    new Genres
                    { 
                        GenreName = "philosophical",
                    },
                    new Genres
                    {
                        GenreName = "realism",
                    },
                    new Genres
                    {
                        GenreName = "historical",
                    },
                    new Genres 
                    {
                        GenreName = "war literature",
                    }
                );
                context.SaveChanges();

                /*context.BookGenre.AddRange(
                    new BookGenre
                    {
                        BookId = 1,
                        GenreId = 1
                    },
                    new BookGenre
                    {
                        BookId = 1,
                        GenreId = 3
                    },
                    new BookGenre
                    {
                        BookId = 1,
                        GenreId = 4
                    },
                    new BookGenre
                    {
                        BookId = 2,
                        GenreId = 1
                    },
                    new BookGenre
                    {
                        BookId = 2,
                        GenreId = 2
                    }
                );
                context.SaveChanges();*/

                context.Review.AddRange(
                    new Review
                    {
                        BookId = 1,
                        AppUser = "User 1",
                        Comment = "Love it",
                        Rating = 5
                    },
                    new Review
                    {
                        BookId = 1,
                        AppUser = "User 2",
                        Comment = "I did not understand it",
                        Rating = 1
                    },
                    new Review
                    {
                        BookId = 2,
                        AppUser = "User 3",
                        Comment = "Really long, but very interesting",
                        Rating = 4
                    },
                    new Review
                    {
                        BookId = 1,
                        AppUser = "User 4",
                        Comment = "I prefer the movie",
                        Rating = 2
                    },
                    new Review
                    {
                        BookId = 1,
                        AppUser = "User 5",
                        Comment = "No comment",
                        Rating = 5
                    },
                    new Review
                    {
                        BookId = 2,
                        AppUser = "User 6",
                        Comment = "Have to read it again",
                        Rating = 5
                    },
                    new Review
                    {
                        BookId = 2,
                        AppUser = "User 5",
                        Comment = "No comment",
                        Rating = 3
                    },
                    new Review
                    {
                        BookId = 1,
                        AppUser = "User 7",
                        Comment = "No comment",
                        Rating = 4
                    }
                );
                context.SaveChanges();

                context.UserBooks.AddRange(
                    new UserBooks
                    {
                        BookId = 1,
                        AppUser = "User 1"
                    },
                    new UserBooks
                    {
                        BookId = 1,
                        AppUser = "User 2"
                    },
                    new UserBooks
                    {
                        BookId = 2,
                        AppUser = "User 3"
                    },
                    new UserBooks
                    {
                        BookId = 1,
                        AppUser = "User 4"
                    },
                    new UserBooks
                    {
                        BookId = 1,
                        AppUser = "User 5"
                    },
                    new UserBooks
                    {
                        BookId = 2,
                        AppUser = "User 5"
                    },
                    new UserBooks
                    {
                        BookId = 2,
                        AppUser = "User 6"
                    },
                    new UserBooks
                    {
                        BookId = 1,
                        AppUser = "User 7"
                    }
                );
                context.SaveChanges();


                /*
                context.Movie.AddRange(
                new Movie
                {
                    //Id = 1,
                    Title = "When Harry Met Sally",
                    ReleaseDate = DateTime.Parse("1989-2-12"),
                    Genre = "Romantic Comedy",
                    Rating = 5,
                    Price = 7.99M,
                    DirectorId = context.Director.Single(d => d.FirstName == "Rob" && d.LastName == "Reiner").Id
                },
                new Movie

                {
                    //Id = 2,
                    Title = "Ghostbusters",
                    ReleaseDate = DateTime.Parse("1984-3
- 13"),
 Genre = "Comedy"
,
Rating = 6,
Price = 8.99M,
DirectorId = context.Director.Single(d => d.FirstName == "Ivan" && d.LastName == "Reitman").Id
 },
new Movie
{
    //Id = 3,
    Title = "Ghostbusters 2"
,
    ReleaseDate = DateTime.Parse
("1986
-
2
- 23"),
 Genre = "Comedy"
,
Rating = 7,
Price = 9.99M,
DirectorId = context.Director.Single(d => d.FirstName == "Ivan" && d.LastName == "Reitman").Id
 },
new Movie
{
    //Id = 4,
    Title = "Rio Bravo"
,
    ReleaseDate = DateTime.Parse
("1959
-
4
- 15"),
 Genre = "Western"
,
Rating = 4,
Price = 3.99M,
DirectorId = context.Director.Single(d => d.FirstName == "Howard" && d.LastName == "Hawks").Id

}
 );
                context.SaveChanges();
                context.ActorMovie.AddRange
               (
                new ActorMovie { ActorId = 1, MovieId = 1 },
                new ActorMovie { ActorId = 2, MovieId = 1 },
                new ActorMovie { ActorId = 3, MovieId = 1 },
                new ActorMovie { ActorId = 4, MovieId = 2 },
                new ActorMovie { ActorId = 5, MovieId = 2 },
                new ActorMovie { ActorId = 6, MovieId = 2 },
                new ActorMovie { ActorId = 4, MovieId = 3 },
                new ActorMovie { ActorId = 5, MovieId = 3 },
                new ActorMovie { ActorId = 6, MovieId = 3 },
                new ActorMovie { ActorId = 7, MovieId = 4 },
                new ActorMovie { ActorId = 8, MovieId = 4 }
                );
                context.SaveChanges();*/

            }

        }

    }
}
