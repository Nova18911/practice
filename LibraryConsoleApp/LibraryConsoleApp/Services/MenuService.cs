using LibraryConsoleApp.Data;
using LibraryConsoleApp.Models;

namespace LibraryConsoleApp.Services
{
    public class MenuService
    {
        private readonly AdoNetRepository _adoRepo;
        private readonly EfRepository _efRepo;
        private bool _useEntityFramework;

        public MenuService(AdoNetRepository adoRepo, EfRepository efRepo, bool useEntityFramework = false)
        {
            _adoRepo = adoRepo;
            _efRepo = efRepo;
            _useEntityFramework = useEntityFramework;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== Library Management System ({(_useEntityFramework ? "Entity Framework" : "ADO.NET")}) ===");
                Console.WriteLine("1. Manage Authors");
                Console.WriteLine("2. Manage Books");
                Console.WriteLine("3. Manage Readers");
                Console.WriteLine("4. Manage Loans");
                Console.WriteLine("5. Switch Data Access Method");
                Console.WriteLine("6. Exit");
                Console.Write("\nSelect option: ");

                var choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        await ManageAuthorsAsync();
                        break;
                    case "2":
                        await ManageBooksAsync();
                        break;
                    case "3":
                        await ManageReadersAsync();
                        break;
                    case "4":
                        await ManageLoansAsync();
                        break;
                    case "5":
                        _useEntityFramework = !_useEntityFramework;
                        Console.WriteLine($"Switched to {(_useEntityFramework ? "Entity Framework" : "ADO.NET")}");
                        Console.ReadKey();
                        break;
                    case "6":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        #region Author Management

        private async Task ManageAuthorsAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Manage Authors ===");
                Console.WriteLine("1. List all authors");
                Console.WriteLine("2. Add new author");
                Console.WriteLine("3. Update author");
                Console.WriteLine("4. Delete author");
                Console.WriteLine("5. Back to main menu");
                Console.Write("\nSelect option: ");

                var choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        await ListAuthorsAsync();
                        break;
                    case "2":
                        await AddAuthorAsync();
                        break;
                    case "3":
                        await UpdateAuthorAsync();
                        break;
                    case "4":
                        await DeleteAuthorAsync();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private async Task ListAuthorsAsync()
        {
            var authors = _useEntityFramework
                ? await _efRepo.GetAllAuthorsAsync()
                : await _adoRepo.GetAllAuthorsAsync();

            Console.WriteLine("=== Authors ===");
            if (authors.Count == 0)
            {
                Console.WriteLine("No authors found.");
            }
            else
            {
                foreach (var author in authors)
                {
                    Console.WriteLine($"ID: {author.Id}");
                    Console.WriteLine($"Name: {author.FullName}");
                    Console.WriteLine($"Birth Date: {author.BirthDate?.ToShortDateString() ?? "Unknown"}");
                    Console.WriteLine($"Country: {author.Country}");
                    Console.WriteLine($"Status: {(author.IsALive ? "Alive" : "Deceased")}");
                    Console.WriteLine("---");
                }
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task AddAuthorAsync()
        {
            Console.WriteLine("=== Add New Author ===");
            var author = new Author();

            Console.Write("Full Name: ");
            author.FullName = Console.ReadLine() ?? string.Empty;

            Console.Write("Birth Date (yyyy-mm-dd, press Enter to skip): ");
            var birthDateStr = Console.ReadLine();
            if (!string.IsNullOrEmpty(birthDateStr) && DateTime.TryParse(birthDateStr, out var birthDate))
            {
                author.BirthDate = birthDate;
            }

            Console.Write("Country: ");
            author.Country = Console.ReadLine() ?? string.Empty;

            Console.Write("Is Alive (y/n): ");
            author.IsALive = Console.ReadLine()?.ToLower() == "y";

            try
            {
                var id = _useEntityFramework
                    ? await _efRepo.CreateAuthorAsync(author)
                    : await _adoRepo.CreateAuthorAsync(author);
                Console.WriteLine($"Author added successfully! ID: {id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task UpdateAuthorAsync()
        {
            Console.WriteLine("=== Update Author ===");
            Console.Write("Enter Author ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid ID format.");
                Console.ReadKey();
                return;
            }

            var author = _useEntityFramework
                ? await _efRepo.GetAuthorByIdAsync(id)
                : await _adoRepo.GetAuthorByIdAsync(id);

            if (author == null)
            {
                Console.WriteLine("Author not found.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Current Name: {author.FullName}");
            Console.Write("New Full Name (press Enter to keep current): ");
            var newName = Console.ReadLine();
            if (!string.IsNullOrEmpty(newName)) author.FullName = newName;

            Console.WriteLine($"Current Country: {author.Country}");
            Console.Write("New Country (press Enter to keep current): ");
            var newCountry = Console.ReadLine();
            if (!string.IsNullOrEmpty(newCountry)) author.Country = newCountry;

            Console.Write($"Is Alive ({author.IsALive}) (y/n, press Enter to keep current): ");
            var aliveInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(aliveInput)) author.IsALive = aliveInput.ToLower() == "y";

            try
            {
                var success = _useEntityFramework
                    ? await _efRepo.UpdateAuthorAsync(author)
                    : await _adoRepo.UpdateAuthorAsync(author);
                Console.WriteLine(success ? "Author updated successfully!" : "Failed to update author.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task DeleteAuthorAsync()
        {
            Console.WriteLine("=== Delete Author ===");
            Console.Write("Enter Author ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid ID format.");
                Console.ReadKey();
                return;
            }

            Console.Write("Are you sure you want to delete this author? (y/n): ");
            if (Console.ReadLine()?.ToLower() != "y")
            {
                Console.WriteLine("Deletion cancelled.");
                Console.ReadKey();
                return;
            }

            try
            {
                var success = _useEntityFramework
                    ? await _efRepo.DeleteAuthorAsync(id)
                    : await _adoRepo.DeleteAuthorAsync(id);
                Console.WriteLine(success ? "Author deleted successfully!" : "Failed to delete author.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        #endregion

        #region Book Management

        private async Task ManageBooksAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Manage Books ===");
                Console.WriteLine("1. List all books");
                Console.WriteLine("2. Add new book");
                Console.WriteLine("3. Update book");
                Console.WriteLine("4. Delete book");
                Console.WriteLine("5. Back to main menu");
                Console.Write("\nSelect option: ");

                var choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        await ListBooksAsync();
                        break;
                    case "2":
                        await AddBookAsync();
                        break;
                    case "3":
                        await UpdateBookAsync();
                        break;
                    case "4":
                        await DeleteBookAsync();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private async Task ListBooksAsync()
        {
            var books = _useEntityFramework
                ? await _efRepo.GetAllBooksAsync()
                : await _adoRepo.GetAllBooksAsync();

            Console.WriteLine("=== Books ===");
            if (books.Count == 0)
            {
                Console.WriteLine("No books found.");
            }
            else
            {
                foreach (var book in books)
                {
                    Console.WriteLine($"ID: {book.Id}");
                    Console.WriteLine($"Title: {book.Title}");
                    Console.WriteLine($"ISBN: {book.ISBN}");
                    Console.WriteLine($"Price: {book.Price:C}");
                    Console.WriteLine($"Published: {book.PublishedAt.ToShortDateString()}");
                    Console.WriteLine($"Status: {(book.InStock ? "In Stock" : "Out of Stock")}");
                    Console.WriteLine("---");
                }
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task AddBookAsync()
        {
            Console.WriteLine("=== Add New Book ===");
            var book = new Book();

            Console.Write("Title: ");
            book.Title = Console.ReadLine() ?? string.Empty;

            Console.Write("ISBN: ");
            book.ISBN = Console.ReadLine() ?? string.Empty;

            Console.Write("Price: ");
            if (!decimal.TryParse(Console.ReadLine(), out var price))
            {
                Console.WriteLine("Invalid price format.");
                Console.ReadKey();
                return;
            }
            book.Price = price;

            Console.Write("Published Date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out var publishedAt))
            {
                Console.WriteLine("Invalid date format.");
                Console.ReadKey();
                return;
            }
            book.PublishedAt = publishedAt;

            Console.Write("In Stock (y/n): ");
            book.InStock = Console.ReadLine()?.ToLower() == "y";

            try
            {
                var id = _useEntityFramework
                    ? await _efRepo.CreateBookAsync(book)
                    : await _adoRepo.CreateBookAsync(book);
                Console.WriteLine($"Book added successfully! ID: {id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task UpdateBookAsync()
        {
            Console.WriteLine("=== Update Book ===");
            Console.Write("Enter Book ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid ID format.");
                Console.ReadKey();
                return;
            }

            var book = _useEntityFramework
                ? await _efRepo.GetBookByIdAsync(id)
                : await _adoRepo.GetBookByIdAsync(id);

            if (book == null)
            {
                Console.WriteLine("Book not found.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Current Title: {book.Title}");
            Console.Write("New Title (press Enter to keep current): ");
            var newTitle = Console.ReadLine();
            if (!string.IsNullOrEmpty(newTitle)) book.Title = newTitle;

            Console.WriteLine($"Current Price: {book.Price:C}");
            Console.Write("New Price (press Enter to keep current): ");
            var priceInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(priceInput) && decimal.TryParse(priceInput, out var newPrice))
                book.Price = newPrice;

            Console.Write($"In Stock ({book.InStock}) (y/n, press Enter to keep current): ");
            var stockInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(stockInput)) book.InStock = stockInput.ToLower() == "y";

            try
            {
                var success = _useEntityFramework
                    ? await _efRepo.UpdateBookAsync(book)
                    : await _adoRepo.UpdateBookAsync(book);
                Console.WriteLine(success ? "Book updated successfully!" : "Failed to update book.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task DeleteBookAsync()
        {
            Console.WriteLine("=== Delete Book ===");
            Console.Write("Enter Book ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid ID format.");
                Console.ReadKey();
                return;
            }

            Console.Write("Are you sure you want to delete this book? (y/n): ");
            if (Console.ReadLine()?.ToLower() != "y")
            {
                Console.WriteLine("Deletion cancelled.");
                Console.ReadKey();
                return;
            }

            try
            {
                var success = _useEntityFramework
                    ? await _efRepo.DeleteBookAsync(id)
                    : await _adoRepo.DeleteBookAsync(id);
                Console.WriteLine(success ? "Book deleted successfully!" : "Failed to delete book.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        #endregion

        #region Reader Management

        private async Task ManageReadersAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Manage Readers ===");
                Console.WriteLine("1. List all readers");
                Console.WriteLine("2. Add new reader");
                Console.WriteLine("3. Update reader");
                Console.WriteLine("4. Delete reader");
                Console.WriteLine("5. Back to main menu");
                Console.Write("\nSelect option: ");

                var choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        await ListReadersAsync();
                        break;
                    case "2":
                        await AddReaderAsync();
                        break;
                    case "3":
                        await UpdateReaderAsync();
                        break;
                    case "4":
                        await DeleteReaderAsync();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private async Task ListReadersAsync()
        {
            var readers = _useEntityFramework
                ? await _efRepo.GetAllReadersAsync()
                : await _adoRepo.GetAllReadersAsync();

            Console.WriteLine("=== Readers ===");
            if (readers.Count == 0)
            {
                Console.WriteLine("No readers found.");
            }
            else
            {
                foreach (var reader in readers)
                {
                    Console.WriteLine($"ID: {reader.Id}");
                    Console.WriteLine($"Name: {reader.FullName}");
                    Console.WriteLine($"Email: {reader.Email}");
                    Console.WriteLine($"Registered: {reader.RegisteredAt}");
                    Console.WriteLine($"Status: {(reader.IsActive ? "Active" : "Inactive")}");
                    Console.WriteLine("---");
                }
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task AddReaderAsync()
        {
            Console.WriteLine("=== Add New Reader ===");
            var reader = new Reader();

            Console.Write("Full Name: ");
            reader.FullName = Console.ReadLine() ?? string.Empty;

            Console.Write("Email: ");
            reader.Email = Console.ReadLine() ?? string.Empty;

            Console.Write("Is Active (y/n): ");
            reader.IsActive = Console.ReadLine()?.ToLower() == "y";
            reader.RegisteredAt = DateTime.Now;

            try
            {
                var id = _useEntityFramework
                    ? await _efRepo.CreateReaderAsync(reader)
                    : await _adoRepo.CreateReaderAsync(reader);
                Console.WriteLine($"Reader added successfully! ID: {id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task UpdateReaderAsync()
        {
            Console.WriteLine("=== Update Reader ===");
            Console.Write("Enter Reader ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid ID format.");
                Console.ReadKey();
                return;
            }

            var reader = _useEntityFramework
                ? await _efRepo.GetReaderByIdAsync(id)
                : await _adoRepo.GetReaderByIdAsync(id);

            if (reader == null)
            {
                Console.WriteLine("Reader not found.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Current Name: {reader.FullName}");
            Console.Write("New Name (press Enter to keep current): ");
            var newName = Console.ReadLine();
            if (!string.IsNullOrEmpty(newName)) reader.FullName = newName;

            Console.WriteLine($"Current Email: {reader.Email}");
            Console.Write("New Email (press Enter to keep current): ");
            var newEmail = Console.ReadLine();
            if (!string.IsNullOrEmpty(newEmail)) reader.Email = newEmail;

            Console.Write($"Is Active ({reader.IsActive}) (y/n, press Enter to keep current): ");
            var activeInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(activeInput)) reader.IsActive = activeInput.ToLower() == "y";

            try
            {
                var success = _useEntityFramework
                    ? await _efRepo.UpdateReaderAsync(reader)
                    : await _adoRepo.UpdateReaderAsync(reader);
                Console.WriteLine(success ? "Reader updated successfully!" : "Failed to update reader.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task DeleteReaderAsync()
        {
            Console.WriteLine("=== Delete Reader ===");
            Console.Write("Enter Reader ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid ID format.");
                Console.ReadKey();
                return;
            }

            Console.Write("Are you sure you want to delete this reader? (y/n): ");
            if (Console.ReadLine()?.ToLower() != "y")
            {
                Console.WriteLine("Deletion cancelled.");
                Console.ReadKey();
                return;
            }

            try
            {
                var success = _useEntityFramework
                    ? await _efRepo.DeleteReaderAsync(id)
                    : await _adoRepo.DeleteReaderAsync(id);
                Console.WriteLine(success ? "Reader deleted successfully!" : "Failed to delete reader.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        #endregion

        #region Loan Management

        private async Task ManageLoansAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Manage Loans ===");
                Console.WriteLine("1. List all loans");
                Console.WriteLine("2. Create new loan");
                Console.WriteLine("3. Return book");
                Console.WriteLine("4. Delete loan");
                Console.WriteLine("5. Back to main menu");
                Console.Write("\nSelect option: ");

                var choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        await ListLoansAsync();
                        break;
                    case "2":
                        await CreateLoanAsync();
                        break;
                    case "3":
                        await ReturnBookAsync();
                        break;
                    case "4":
                        await DeleteLoanAsync();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private async Task ListLoansAsync()
        {
            var loans = _useEntityFramework
                ? await _efRepo.GetAllLoansAsync()
                : await _adoRepo.GetAllLoansAsync();

            Console.WriteLine("=== Loans ===");
            if (loans.Count == 0)
            {
                Console.WriteLine("No loans found.");
            }
            else
            {
                foreach (var loan in loans)
                {
                    Console.WriteLine($"ID: {loan.Id}");
                    Console.WriteLine($"Book ID: {loan.BookId}");
                    Console.WriteLine($"Reader ID: {loan.ReaderId}");
                    Console.WriteLine($"Loaned At: {loan.LoanedAt}");
                    if (loan.ReturnedAt.HasValue)
                        Console.WriteLine($"Returned At: {loan.ReturnedAt}");
                    Console.WriteLine($"Status: {(loan.IsReturned ? "Returned" : "Active")}");
                    Console.WriteLine("---");
                }
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task CreateLoanAsync()
        {
            Console.WriteLine("=== Create New Loan ===");
            var loan = new Loan();

            Console.Write("Book ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out var bookId))
            {
                Console.WriteLine("Invalid Book ID format.");
                Console.ReadKey();
                return;
            }
            loan.BookId = bookId;

            Console.Write("Reader ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out var readerId))
            {
                Console.WriteLine("Invalid Reader ID format.");
                Console.ReadKey();
                return;
            }
            loan.ReaderId = readerId;

            loan.LoanedAt = DateTime.Now;
            loan.IsReturned = false;

            try
            {
                var id = _useEntityFramework
                    ? await _efRepo.CreateLoanAsync(loan)
                    : await _adoRepo.CreateLoanAsync(loan);
                Console.WriteLine($"Loan created successfully! ID: {id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task ReturnBookAsync()
        {
            Console.WriteLine("=== Return Book ===");
            Console.Write("Enter Loan ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid ID format.");
                Console.ReadKey();
                return;
            }

            try
            {
                var success = _useEntityFramework
                    ? await _efRepo.ReturnBookAsync(id)
                    : await _adoRepo.ReturnBookAsync(id);
                Console.WriteLine(success ? "Book returned successfully!" : "Failed to return book.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task DeleteLoanAsync()
        {
            Console.WriteLine("=== Delete Loan ===");
            Console.Write("Enter Loan ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid ID format.");
                Console.ReadKey();
                return;
            }

            Console.Write("Are you sure you want to delete this loan? (y/n): ");
            if (Console.ReadLine()?.ToLower() != "y")
            {
                Console.WriteLine("Deletion cancelled.");
                Console.ReadKey();
                return;
            }

            try
            {
                var success = _useEntityFramework
                    ? await _efRepo.DeleteLoanAsync(id)
                    : await _adoRepo.DeleteLoanAsync(id);
                Console.WriteLine(success ? "Loan deleted successfully!" : "Failed to delete loan.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        #endregion
    }
}