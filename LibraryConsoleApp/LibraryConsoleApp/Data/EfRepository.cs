using Microsoft.EntityFrameworkCore;
using LibraryConsoleApp.Models;

namespace LibraryConsoleApp.Data
{
    public class EfRepository : IDisposable
    {
        private readonly LibraryDbContext _context;

        public EfRepository(LibraryDbContext context)
        {
            _context = context;
        }

        #region Authors CRUD

        public async Task<Author?> GetAuthorByIdAsync(Guid id)
        {
            return await _context.Authors.FindAsync(id);
        }

        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            return await _context.Authors
                .OrderBy(a => a.FullName)
                .ToListAsync();
        }

        public async Task<Guid> CreateAuthorAsync(Author author)
        {
            author.Id = Guid.NewGuid();
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
            return author.Id;
        }

        public async Task<bool> UpdateAuthorAsync(Author author)
        {
            _context.Authors.Update(author);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAuthorAsync(Guid id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return false;

            _context.Authors.Remove(author);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        #endregion

        #region Books CRUD

        public async Task<Book?> GetBookByIdAsync(Guid id)
        {
            return await _context.Books
                .Include(b => b.BookAuthors)
                .Include(b => b.BookGenres)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books
                .Include(b => b.BookAuthors)
                .Include(b => b.BookGenres)
                .OrderBy(b => b.Title)
                .ToListAsync();
        }

        public async Task<Guid> CreateBookAsync(Book book)
        {
            book.Id = Guid.NewGuid();
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book.Id;
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteBookAsync(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            _context.Books.Remove(book);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        #endregion

        #region Readers CRUD

        public async Task<Reader?> GetReaderByIdAsync(Guid id)
        {
            return await _context.Readers
                .Include(r => r.Loans)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Reader>> GetAllReadersAsync()
        {
            return await _context.Readers
                .OrderBy(r => r.FullName)
                .ToListAsync();
        }

        public async Task<Guid> CreateReaderAsync(Reader reader)
        {
            reader.Id = Guid.NewGuid();
            await _context.Readers.AddAsync(reader);
            await _context.SaveChangesAsync();
            return reader.Id;
        }

        public async Task<bool> UpdateReaderAsync(Reader reader)
        {
            _context.Readers.Update(reader);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteReaderAsync(Guid id)
        {
            var reader = await _context.Readers.FindAsync(id);
            if (reader == null) return false;

            _context.Readers.Remove(reader);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        #endregion

        #region Loans CRUD

        public async Task<Loan?> GetLoanByIdAsync(Guid id)
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Reader)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<List<Loan>> GetAllLoansAsync()
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Reader)
                .OrderByDescending(l => l.LoanedAt)
                .ToListAsync();
        }

        public async Task<Guid> CreateLoanAsync(Loan loan)
        {
            loan.Id = Guid.NewGuid();
            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();
            return loan.Id;
        }

        public async Task<bool> ReturnBookAsync(Guid loanId)
        {
            var loan = await _context.Loans.FindAsync(loanId);
            if (loan == null) return false;

            loan.IsReturned = true;
            loan.ReturnedAt = DateTime.Now;

            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteLoanAsync(Guid id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null) return false;

            _context.Loans.Remove(loan);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        #endregion

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}