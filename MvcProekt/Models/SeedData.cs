﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcProekt.Areas.Identity.Data;
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
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<MvcProektUser>>();
            IdentityResult roleResult;
            var registeredRoleCheck = await RoleManager.RoleExistsAsync("Registered");
            if (!registeredRoleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Registered"));
            }
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            MvcProektUser user = await UserManager.FindByEmailAsync("admin@mvcmovie.com");
            if (user == null)
            {
                var User = new MvcProektUser();
                User.Email = "admin@mvcmovie.com";
                User.UserName = "admin@mvcmovie.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }

            var allUsers = await UserManager.Users.ToListAsync();
            foreach (var usr in allUsers)
            {
                if (!await UserManager.IsInRoleAsync(usr, "Registered"))
                {
                    await UserManager.AddToRoleAsync(usr, "Registered");
                }
            }
        }
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MvcProektContext(
            serviceProvider.GetRequiredService<
            DbContextOptions<MvcProektContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();

                if (context.Books.Any() || context.Author.Any() || context.Genres.Any() || context.UserBooks.Any() || context.BookGenre.Any())
                {
                    return;
                }

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

                context.Books.AddRange(
                    new Books 
                    { 
                        Title = "War and Peace", 
                        YearPublished = 1986, 
                        NumPages = 1200,  
                        Description = "\"War and Peace\" is an epic novel by Leo Tolstoy that chronicles the lives of Russian aristocrats against the backdrop of Napoleon's invasion of Russia, exploring themes of love, war, politics, and the human condition.", 
                        Publisher = "Russkiy Vestnik", 
                        AuthorId = 1, 
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
                        AuthorId = 2,
                        DownloadUrl = "https://www.planetebook.com/free-ebooks/crime-and-punishment.pdf",
                        FrontPage = "https://qph.cf2.quoracdn.net/main-qimg-60ca2932975ad8fac18089434f1c0d3f-lq"
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

                context.BookGenre.AddRange(
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

                context.SaveChanges();

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
                    
                );
                context.SaveChanges();
            }

        }

    }
}
