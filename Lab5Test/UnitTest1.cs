using CSCI2910_Lab5.Models;
using CSCI2910_Lab5.Services;

[TestClass]
public class LibraryServiceTests
{
    private LibraryService _libraryService;
    private string _booksFilePath = "../../../../CSCI2910_Lab5/Data/Books.csv";
    private string _usersFilePath = "../../../../CSCI2910_Lab5/Data/Users.csv";
    private string _borrowedBooksFilePath = "../../../../CSCI2910_Lab5/Data/BorrowedBooks.csv";

    [TestInitialize]
    public void Setup()
    {
        // Setup the service with temporary file paths for isolation
        _libraryService = new LibraryService(_booksFilePath, _usersFilePath, _borrowedBooksFilePath);
    }

    [TestMethod]
    public void AddBook_ShouldAddBookToList()
    {
        // Arrange
        var testBook = new Book { Title = "Donalds Book", Author = "Donald Duck", ISBN = "1234567890" };

        // Act
        _libraryService.AddBook(testBook);

        // Assert
        Assert.IsNotNull(_libraryService.GetBookByTitle("Donalds Book"));
    }

    [TestMethod]
    public void GetBookByTitle_ShouldReturnCorrectBook()
    {
        // Arrange
        var book = new Book { Title = "Test Book 2", Author = "Another Author", ISBN = "0987654321" };
        _libraryService.AddBook(book);

        // Assert
        Assert.IsNotNull(_libraryService.GetBookByTitle("Test Book 2"));
    }

    [TestMethod]
    public void GetBookByTitle_ShouldNotReturnBook()
    {
        // Act
        Book book1 = _libraryService.GetBookByTitle("jfjdsjjdsfjkshdjfhjdskhfd");

        // Assert
        Assert.IsNull(book1);
    }

    // Delete a book id which exists
    [TestMethod]
    public void DeleteBook_ShouldRemoveBook()
    {
        // Arrange
        var book = new Book { Title = "To Be Deleted", Author = "Author", ISBN = 777888999 };
        _libraryService.AddBook(book);

        // Act
        _libraryService.DeleteBook(1);
        var result = _libraryService.GetBookById(1);

        // Assert
        Assert.IsNull(result);
    }

    // Delete a book id which does not exist
    [TestMethod]
    public void DeleteBook_ShouldNotRemoveInvalidBook()
    {
        // Arrange
        int bookCount = _libraryService.GetAllBooks().Count;

        // Act
        _libraryService.DeleteBook(432984324);

        // Assert
        Assert.IsTrue(_libraryService.GetAllBooks().Count == bookCount);
    }

    [TestMethod]
    public void AddUser_ShouldAddUserToList()
    {
        // Arrange
        var testUser = new User { Name = "Test User", Email = "test@example.com" };

        // Act
        _libraryService.AddUser(testUser);

        // Assert
        Assert.IsNotNull(_libraryService.GetUserByName("Test User"));
    }
}
