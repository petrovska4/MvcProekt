using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcProekt.Models;

namespace MvcProekt.Data
{
    public class MvcProektContext : DbContext
    {
        public MvcProektContext(DbContextOptions<MvcProektContext> options)
            : base(options)
        {
        }

        public DbSet<MvcProekt.Models.Books> Books { get; set; } = default!;
        public DbSet<MvcProekt.Models.Author> Author { get; set; } = default!;
        public DbSet<MvcProekt.Models.BookGenre> BookGenre { get; set; } = default!;
        public DbSet<MvcProekt.Models.Genres> Genres { get; set; } = default!;
        public DbSet<MvcProekt.Models.Review> Review { get; set; } = default!;
        public DbSet<MvcProekt.Models.UserBooks> UserBooks { get; set; } = default!;


        /*protected override void OnModelCreating(ModelBuilder builder)
        {
            *//*builder.Entity<Books>()
                .HasOne<Author>(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId);

            builder.Entity<BookGenre>()
                .HasOne(bg => bg.Book)
                .WithMany(b => b.BookGenres)
                .HasForeignKey(bg => bg.BookId);

            builder.Entity<BookGenre>()
                .HasOne(bg => bg.Genre)
                .WithMany(g => g.BookGenres)
                .HasForeignKey(bg => bg.GenreId);

            builder.Entity<Review>()
                .HasOne<Books>(r => r.Book)
                .WithMany(b => b.Reviews)
                .HasForeignKey(r => r.BookId);

            builder.Entity<UserBooks>()
                .HasOne<Books>(ub => ub.Book)
                .WithMany(b => b.UserBooks)
                .HasForeignKey(ub => ub.BookId);*//*

        }*/
    }
}