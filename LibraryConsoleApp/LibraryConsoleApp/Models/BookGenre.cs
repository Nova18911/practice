using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryConsoleApp.Models
{
    [Table("bookGenres")]
    public class BookGenre
    {
        public Guid BookId { get; set; }

        public Guid GenreId { get; set; }

        [ForeignKey("BookId")]
        public Book? Book { get; set; }

        [ForeignKey("GenreId")]
        public Genre? Genre { get; set; }
    }
}