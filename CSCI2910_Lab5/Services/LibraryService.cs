using CSCI2910_Lab5.Models;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using static System.Reflection.Metadata.BlobBuilder;

namespace CSCI2910_Lab5.Services
{

    public class LibraryService : ILibraryService
    {
        private string _booksFilePath;
        private string _usersFilePath;
        private string _borrowedBooksFilePath;

        public LibraryService()
        {
            _booksFilePath = "Data/Books.csv";
            _usersFilePath = "Data/Users.csv";
            _borrowedBooksFilePath = "Data/BorrowedBooks.csv";
            EnsureCsvFilesExist();
        }

        public LibraryService(string booksFilePath, string usersFilePath, string borrowedBooksFilePath)
        {
            _booksFilePath = booksFilePath;
            _usersFilePath = usersFilePath;
            _borrowedBooksFilePath = borrowedBooksFilePath;
            EnsureCsvFilesExist();
        }

        private void EnsureCsvFilesExist()
        {
            // Ensure Books CSV
            if (!File.Exists(_booksFilePath))
            {
                using var writer = new StreamWriter(_booksFilePath);
                writer.WriteLine("Id,Title,Author,ISBN");
            }
            else
            {
                // Check if the existing file has headers
                var firstLine = File.ReadLines(_booksFilePath).FirstOrDefault();
                if (firstLine != "Id,Title,Author,ISBN")
                {
                    // Rewrite file with correct headers if headers are incorrect
                    var lines = File.ReadAllLines(_booksFilePath).ToList();
                    lines.Insert(0, "Id,Title,Author,ISBN");
                    File.WriteAllLines(_booksFilePath, lines);
                }
            }

            // Ensure Users CSV
            if (!File.Exists(_usersFilePath))
            {
                using var writer = new StreamWriter(_usersFilePath);
                writer.WriteLine("Id,Name,Email");
            }
            else
            {
                // Check if the existing file has headers
                var firstLine = File.ReadLines(_usersFilePath).FirstOrDefault();
                if (firstLine != "Id,Name,Email")
                {
                    // Rewrite file with correct headers if headers are incorrect
                    var lines = File.ReadAllLines(_usersFilePath).ToList();
                    lines.Insert(0, "Id,Name,Email");
                    File.WriteAllLines(_usersFilePath, lines);
                }
            }

            // Ensure BorrowedBooks CSV
            if (!File.Exists(_borrowedBooksFilePath))
            {
                using var writer = new StreamWriter(_borrowedBooksFilePath);
                writer.WriteLine("Id,BookId,UserId,DateBorrowed,DateReturned");
            }
            else
            {
                // Check if the existing file has headers
                var firstLine = File.ReadLines(_borrowedBooksFilePath).FirstOrDefault();
                if (firstLine != "Id,BookId,UserId,DateBorrowed,DateReturned")
                {
                    // Rewrite file with correct headers if headers are incorrect
                    var lines = File.ReadAllLines(_borrowedBooksFilePath).ToList();
                    lines.Insert(0, "Id,BookId,UserId,DateBorrowed,DateReturned");
                    File.WriteAllLines(_borrowedBooksFilePath, lines);
                }
            }
        }

        // Book methods
        public List<Book> GetAllBooks()
        {
            return ReadBooksFromCsv();
        }

        public Book GetBookById(int id)
        {
            try
            {
                return ReadBooksFromCsv().FirstOrDefault(b => b.Id == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int GetBookIdByTitle(string title)
        {
            var book = ReadBooksFromCsv().FirstOrDefault(b => b.Title == title);
            return book != null ? book.Id : -1;
        }

        public Book GetBookByTitle(string title)
        {
            return ReadBooksFromCsv().FirstOrDefault(b => b.Title == title);
        }

        public void AddBook(Book book)
        {
            var books = ReadBooksFromCsv();
            book.Id = books.Count > 0 ? books.Max(b => b.Id) + 1 : 1;
            books.Add(book);
            WriteBooksToCsv(books);
        }

        public void UpdateBook(Book book)
        {
            var books = ReadBooksFromCsv();
            var index = books.FindIndex(b => b.Id == book.Id);
            if (index >= 0)
            {
                books[index] = book;
                WriteBooksToCsv(books);
            }
        }

        public void DeleteBook(int id)
        {
            var books = ReadBooksFromCsv();
            books.RemoveAll(b => b.Id == id);
            WriteBooksToCsv(books);
        }

        // User methods
        public List<User> GetAllUsers()
        {
            return ReadUsersFromCsv();
        }

        public User GetUserById(int id)
        {
            return ReadUsersFromCsv().FirstOrDefault(u => u.Id == id);
        }

        public int GetUserIdByName(string name)
        {
            var user = ReadUsersFromCsv().FirstOrDefault(u => u.Name == name);
            return user != null ? user.Id : -1;
        }

        public string GetUserNameById(int id)
        {
			var user = ReadUsersFromCsv().FirstOrDefault(u => u.Id == id);
			return user != null ? user.Name : null;
		}

        public User GetUserByName(string name)
        {
            return ReadUsersFromCsv().FirstOrDefault(u => u.Name == name);
        }

        public void AddUser(User user)
        {
            var users = ReadUsersFromCsv();
            user.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
            users.Add(user);
            WriteUsersToCsv(users);
        }

        public void UpdateUser(User user)
        {
            var users = ReadUsersFromCsv();
            var index = users.FindIndex(u => u.Id == user.Id);
            if (index >= 0)
            {
                users[index] = user;
                WriteUsersToCsv(users);
            }
        }

        public void DeleteUser(int id)
        {
            var users = ReadUsersFromCsv();
            users.RemoveAll(u => u.Id == id);
            WriteUsersToCsv(users);
        }

        // BorrowedBook methods

        public List<BorrowedBook> GetBorrowedBooks()
        {
            return ReadBorrowedBooksFromCsv();
        }

        public BorrowedBook GetBorrowedBookByUserName(string name)
        {
            var user = ReadUsersFromCsv().FirstOrDefault(u => u.Name == name);
            if (user != null)
            {
                return ReadBorrowedBooksFromCsv().FirstOrDefault(b => b.UserId == user.Id && b.DateReturned == null);
            }
            return null;
        }

        public BorrowedBook GetBorrowedBookByTitle(string title)
        {
            var book = ReadBooksFromCsv().FirstOrDefault(b => b.Title == title);
            if (book != null)
            {
                return ReadBorrowedBooksFromCsv().FirstOrDefault(b => b.BookId == book.Id && b.DateReturned == null);
            }
            return null;
        }
        public void BorrowBook(int bookId, int userId)
        {
            var borrowedBooks = ReadBorrowedBooksFromCsv();
            borrowedBooks.Add(new BorrowedBook
            {
                Id = borrowedBooks.Count + 1,
                BookId = bookId,
                UserId = userId,
                DateBorrowed = DateTime.Now
            });
            WriteBorrowedBooksToCsv(borrowedBooks);
        }

        public void ReturnBook(int bookId)
        {
            var borrowedBooks = ReadBorrowedBooksFromCsv();
            var borrowedBook = borrowedBooks.FirstOrDefault(b => b.BookId == bookId && b.DateReturned == null);
            if (borrowedBook != null)
            {
                borrowedBook.DateReturned = DateTime.Now;
                WriteBorrowedBooksToCsv(borrowedBooks);
            }
        }

        // Helper methods for CSV handling
        private List<Book> ReadBooksFromCsv()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null, // Disable header validation
                MissingFieldFound = null // Disable missing field validation
            };
            using var reader = new StreamReader(_booksFilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<Book>().ToList();
        }

        private void WriteBooksToCsv(List<Book> books)
        {
            using var writer = new StreamWriter(_booksFilePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteHeader<Book>();
            csv.NextRecord();
            csv.WriteRecords(books);
        }

        private List<User> ReadUsersFromCsv()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null, // Disable header validation
                MissingFieldFound = null // Disable missing field validation
            };
            using var reader = new StreamReader(_usersFilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<User>().ToList();
        }

        private void WriteUsersToCsv(List<User> users)
        {
            using var writer = new StreamWriter(_usersFilePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteHeader<User>();
            csv.NextRecord();
            csv.WriteRecords(users);
        }

        private List<BorrowedBook> ReadBorrowedBooksFromCsv()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null, // Disable header validation
                MissingFieldFound = null // Disable missing field validation
            };
            using var reader = new StreamReader(_borrowedBooksFilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<BorrowedBook>().ToList();
        }

        private void WriteBorrowedBooksToCsv(List<BorrowedBook> borrowedBooks)
        {
            using var writer = new StreamWriter(_borrowedBooksFilePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteHeader<BorrowedBook>();
            csv.NextRecord();
            csv.WriteRecords(borrowedBooks);
        }
    }
}
