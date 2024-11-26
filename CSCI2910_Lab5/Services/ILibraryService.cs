using CSCI2910_Lab5.Models;

namespace CSCI2910_Lab5.Services
{
    public interface ILibraryService
    {
        // Book methods
        List<Book> GetAllBooks();
        Book GetBookById(int id);
        void AddBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(int id);

        // User methods
        List<User> GetAllUsers();
        User GetUserById(int id);
        User GetUserByName(string name);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}
