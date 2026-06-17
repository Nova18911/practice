using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryConsoleApp.Models
{
    [Table("bookAuthors")]
    public class BookAuthor
    {
        public Guid BookId { get; set; }

        public Guid AuthorId { get; set; }

        [ForeignKey("BookId")]
        public Book? Book { get; set; }

        [ForeignKey("AuthorId")]
        public Author? Author { get; set; }
    }
}