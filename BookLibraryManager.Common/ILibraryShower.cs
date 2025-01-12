namespace BookLibraryManager.Common;

/// <author>YR 2025-01-09</author>
public interface ILibraryShower
{
    List<Book> GetFirstBooks(int amountFirstBooks);

    List<Book> GetAllBooks();

    int NumberOfBooks { get; }

    string ShowFistBooks(int amountFirstBooks);
  
    string ShowLastBooks(int amountLastBooks);
}
