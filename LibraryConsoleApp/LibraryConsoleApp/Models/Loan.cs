using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryConsoleApp.Models
{
    [Table("Loans")]
    public class Loan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid BookId { get; set; }

        public Guid ReaderId { get; set; }

        public DateTime LoanedAt { get; set; } = DateTime.Now;

        public DateTime? ReturnedAt { get; set; }

        public bool IsReturned { get; set; } = false;

        [ForeignKey("BookId")]
        public Book? Book { get; set; }

        [ForeignKey("ReaderId")]
        public Reader? Reader { get; set; }
    }
}