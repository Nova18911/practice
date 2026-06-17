using LibraryConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace LibraryConsoleApp.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка составных ключей
            modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => new { ba.BookId, ba.AuthorId });

            modelBuilder.Entity<BookGenre>()
                .HasKey(bg => new { bg.BookId, bg.GenreId });

            // Настройка связей
            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.BookId);

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Author)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(ba => ba.AuthorId);

            modelBuilder.Entity<BookGenre>()
                .HasOne(bg => bg.Book)
                .WithMany(b => b.BookGenres)
                .HasForeignKey(bg => bg.BookId);

            modelBuilder.Entity<BookGenre>()
                .HasOne(bg => bg.Genre)
                .WithMany(g => g.BookGenres)
                .HasForeignKey(bg => bg.GenreId);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BookId);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Reader)
                .WithMany(r => r.Loans)
                .HasForeignKey(l => l.ReaderId);

            // Индексы
            modelBuilder.Entity<Book>()
                .HasIndex(b => b.ISBN)
                .IsUnique()
                .HasDatabaseName("UX_Books_ISBN");

            modelBuilder.Entity<Reader>()
                .HasIndex(r => r.Email)
                .IsUnique()
                .HasDatabaseName("UX_Readers_Email");

            // Установка значений по умолчанию
            modelBuilder.Entity<Author>()
                .Property(a => a.IsALive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Reader>()
                .Property(r => r.RegisteredAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Reader>()
                .Property(r => r.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Book>()
                .Property(b => b.InStock)
                .HasDefaultValue(true);

            modelBuilder.Entity<Loan>()
                .Property(l => l.LoanedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Loan>()
                .Property(l => l.IsReturned)
                .HasDefaultValue(false);
        }
    }
}