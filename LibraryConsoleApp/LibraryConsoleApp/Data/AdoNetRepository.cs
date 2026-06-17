using System.Data;
using Microsoft.Data.SqlClient; // Используем Microsoft.Data.SqlClient вместо System.Data.SqlClient
using LibraryConsoleApp.Models;

namespace LibraryConsoleApp.Data
{
    public class AdoNetRepository : IDisposable
    {
        private readonly string _connectionString;
        private SqlConnection? _connection;

        public AdoNetRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection GetConnection()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
            }
            return _connection;
        }

        #region Authors CRUD

        public async Task<Author?> GetAuthorByIdAsync(Guid id)
        {
            const string sql = "SELECT * FROM authors WHERE Id = @Id";
            using var command = new SqlCommand(sql, GetConnection());
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapAuthor(reader);
            }
            return null;
        }

        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            const string sql = "SELECT * FROM authors ORDER BY FullName";
            using var command = new SqlCommand(sql, GetConnection());
            using var reader = await command.ExecuteReaderAsync();

            var authors = new List<Author>();
            while (await reader.ReadAsync())
            {
                authors.Add(MapAuthor(reader));
            }
            return authors;
        }

        public async Task<Guid> CreateAuthorAsync(Author author)
        {
            author.Id = Guid.NewGuid();
            const string sql = @"
                INSERT INTO authors (Id, FullName, BirthDate, Country, IsALive) 
                VALUES (@Id, @FullName, @BirthDate, @Country, @IsALive)";

            using var command = new SqlCommand(sql, GetConnection());
            command.Parameters.AddWithValue("@Id", author.Id);
            command.Parameters.AddWithValue("@FullName", author.FullName);
            command.Parameters.AddWithValue("@BirthDate", (object?)author.BirthDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@Country", author.Country);
            command.Parameters.AddWithValue("@IsALive", author.IsALive);

            await command.ExecuteNonQueryAsync();
            return author.Id;
        }

        public async Task<bool> UpdateAuthorAsync(Author author)
        {
            const string sql = @"
                UPDATE authors 
                SET FullName = @FullName, BirthDate = @BirthDate, 
                    Country = @Country, IsALive = @IsALive
                WHERE Id = @Id";

            using var command = new SqlCommand(sql, GetConnection());
            command.Parameters.AddWithValue("@Id", author.Id);
            command.Parameters.AddWithValue("@FullName", author.FullName);
            command.Parameters.AddWithValue("@BirthDate", (object?)author.BirthDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@Country", author.Country);
            command.Parameters.AddWithValue("@IsALive", author.IsALive);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAuthorAsync(Guid id)
        {
            const string sql = "DELETE FROM authors WHERE Id = @Id";
            using var command = new SqlCommand(sql, GetConnection());
            command.Parameters.AddWithValue("@Id", id);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        private static Author MapAuthor(SqlDataReader reader)
        {
            return new Author
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                BirthDate = reader.IsDBNull(reader.GetOrdinal("BirthDate"))
                    ? null : reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                Country = reader.GetString(reader.GetOrdinal("Country")),
                IsALive = reader.GetBoolean(reader.GetOrdinal("IsALive"))
            };
        }

        #endregion

        #region Books CRUD

        public async Task<Book?> GetBookByIdAsync(Guid id)
        {
            const string sql = "SELECT * FROM books WHERE Id = @Id";
            using var command = new SqlCommand(sql, GetConnection());
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapBook(reader);
            }
            return null;
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            const string sql = "SELECT * FROM books ORDER BY Title";
            using var command = new SqlCommand(sql, GetConnection());
            using var reader = await command.ExecuteReaderAsync();

            var books = new List<Book>();
            while (await reader.ReadAsync())
            {
                books.Add(MapBook(reader));
            }
            return books;
        }

        public async Task<Guid> CreateBookAsync(Book book)
        {
            book.Id = Guid.NewGuid();
            const string sql = @"
                INSERT INTO books (Id, Title, ISBN, Price, PublishedAt, InStock) 
                VALUES (@Id, @Title, @ISBN, @Price, @PublishedAt, @InStock)";

            using var command = new SqlCommand(sql, GetConnection());
            command.Parameters.AddWithValue("@Id", book.Id);
            command.Parameters.AddWithValue("@Title", book.Title);
            command.Parameters.AddWithValue("@ISBN", book.ISBN);
            command.Parameters.AddWithValue("@Price", book.Price);
            command.Parameters.AddWithValue("@PublishedAt", book.PublishedAt);
            command.Parameters.AddWithValue("@InStock", book.InStock);

            await command.ExecuteNonQueryAsync();
            return book.Id;
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            const string sql = @"
                UPDATE books 
                SET Title = @Title, ISBN = @ISBN, Price = @Price, 
                    PublishedAt = @PublishedAt, InStock = @InStock
                WHERE Id = @Id";

            using var command = new SqlCommand(sql, GetConnection());
            command.Parameters.AddWithValue("@Id", book.Id);
            command.Parameters.AddWithValue("@Title", book.Title);
            command.Parameters.AddWithValue("@ISBN", book.ISBN);
            command.Parameters.AddWithValue("@Price", book.Price);
            command.Parameters.AddWithValue("@PublishedAt", book.PublishedAt);
            command.Parameters.AddWithValue("@InStock", book.InStock);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteBookAsync(Guid id)
        {
            const string sql = "DELETE FROM books WHERE Id = @Id";
            using var command = new SqlCommand(sql, GetConnection());
            command.Parameters.AddWithValue("@Id", id);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        private static Book MapBook(SqlDataReader reader)
        {
            return new Book
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                ISBN = reader.GetString(reader.GetOrdinal("ISBN")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                PublishedAt = reader.GetDateTime(reader.GetOrdinal("PublishedAt")),
                InStock = reader.GetBoolean(reader.GetOrdinal("InStock"))
            };
        }

        #endregion

        #region Readers CRUD

        public async Task<Reader?> GetReaderByIdAsync(Guid id)
        {
            const string sql = "SELECT * FROM readers WHERE Id = @Id";
            using var command = new SqlCommand(sql, GetConnection());
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapReader(reader);
            }
            return null;
        }

        public async Task<List<Reader>> GetAllReadersAsync()
        {
            const string sql = "SELECT * FROM readers ORDER BY FullName";
            using var command = new SqlCommand(sql, GetConnection());
            using var reader = await command.ExecuteReaderAsync();

            var readers = new List<Reader>();
            while (await reader.ReadAsync())
            {
                readers.Add(MapReader(reader));
            }
            return readers;
        }

        public async Task<Guid> CreateReaderAsync(Reader reader)
        {
            reader.Id = Guid.NewGuid();
            const string sql = @"
                INSERT INTO readers (Id, FullName, Email, RegisteredAt, IsActive) 
                VALUES (@Id, @FullName, @Email, @RegisteredAt, @IsActive)";

            using var command = new SqlCommand(sql, GetConnection());
            command.Parameters.AddWithValue("@Id", reader.Id);
            command.Parameters.AddWithValue("@FullName", reader.FullName);
            command.Parameters.AddWithValue("@Email", reader.Email);
            command.Parameters.AddWithValue("@RegisteredAt", reader.RegisteredAt);
            command.Parameters.AddWithValue("@IsActive", reader.IsActive);

            await command.ExecuteNonQueryAsync();
            return reader.Id;
        }

        public async Task<bool> UpdateReaderAsync(Reader reader)
        {
            const string sql = @"
                UPDATE readers 
                SET FullName = @FullName, Email = @Email, 
                    RegisteredAt = @RegisteredAt, IsActive = @IsActive
                WHERE Id = @Id";

            using var command = new SqlCommand(sql, GetConnection());
            command.Parameters.AddWithValue("@Id", reader.Id);
            command.Parameters.AddWithValue("@FullName", reader.FullName);
            command.Parameters.AddWithValue("@Email", reader.Email);
            command.Parameters.AddWithValue("@RegisteredAt", reader.RegisteredAt);
            command.Parameters.AddWithValue("@IsActive", reader.IsActive);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteReaderAsync(Guid id)
        {
            const string sql = "DELETE FROM readers WHERE Id = @Id";
            using var command = new SqlCommand(sql, GetConnection());
            command.Parameters.AddWithValue("@Id", id);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        private static Reader MapReader(SqlDataReader reader)
        {
            return new Reader
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                RegisteredAt = reader.GetDateTime(reader.GetOrdinal("RegisteredAt")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            };
        }

        #endregion

        #region Loans CRUD

        public async Task<Loan?> GetLoanByIdAsync(Guid id)
        {
            const string sql = "SELECT * FROM Loans WHERE Id = @Id";
            using var command = new SqlCommand(sql, GetConnection());
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapLoan(reader);
            }
            return null;
        }

        public async Task<List<Loan>> GetAllLoansAsync()
        {
            const string sql = "SELECT * FROM Loans ORDER BY LoanedAt DESC";
            using var command = new SqlCommand(sql, GetConnection());
            using var reader = await command.ExecuteReaderAsync();

            var loans = new List<Loan>();
            while (await reader.ReadAsync())
            {
                loans.Add(MapLoan(reader));
            }
            return loans;
        }

        public async Task<Guid> CreateLoanAsync(Loan loan)
        {
            loan.Id = Guid.NewGuid();
            const string sql = @"
                INSERT INTO Loans (Id, BookId, ReaderId, LoanedAt, ReturnedAt, IsReturned) 
                VALUES (@Id, @BookId, @ReaderId, @LoanedAt, @ReturnedAt, @IsReturned)";

            using var command = new SqlCommand(sql, GetConnection());
            command.Parameters.AddWithValue("@Id", loan.Id);
            command.Parameters.AddWithValue("@BookId", loan.BookId);
            command.Parameters.AddWithValue("@ReaderId", loan.ReaderId);
            command.Parameters.AddWithValue("@LoanedAt", loan.LoanedAt);
            command.Parameters.AddWithValue("@ReturnedAt", (object?)loan.ReturnedAt ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsReturned", loan.IsReturned);

            await command.ExecuteNonQueryAsync();
            return loan.Id;
        }

        public async Task<bool> ReturnBookAsync(Guid loanId)
        {
            const string sql = @"
                UPDATE Loans 
                SET IsReturned = 1, ReturnedAt = GETDATE()
                WHERE Id = @Id";

            using var command = new SqlCommand(sql, GetConnection());
            command.Parameters.AddWithValue("@Id", loanId);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteLoanAsync(Guid id)
        {
            const string sql = "DELETE FROM Loans WHERE Id = @Id";
            using var command = new SqlCommand(sql, GetConnection());
            command.Parameters.AddWithValue("@Id", id);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        private static Loan MapLoan(SqlDataReader reader)
        {
            return new Loan
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                BookId = reader.GetGuid(reader.GetOrdinal("BookId")),
                ReaderId = reader.GetGuid(reader.GetOrdinal("ReaderId")),
                LoanedAt = reader.GetDateTime(reader.GetOrdinal("LoanedAt")),
                ReturnedAt = reader.IsDBNull(reader.GetOrdinal("ReturnedAt"))
                    ? null : reader.GetDateTime(reader.GetOrdinal("ReturnedAt")),
                IsReturned = reader.GetBoolean(reader.GetOrdinal("IsReturned"))
            };
        }

        #endregion

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}