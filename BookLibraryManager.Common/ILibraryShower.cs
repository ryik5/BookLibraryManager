namespace BookLibraryManager.Common;

public interface ILibraryShower
{
    List<Book> GetFirstBooks(int amountFirstBooks);

    int NumberOfBooks { get; }

    string ShowFistBooks(int amountFirstBooks);
  
    string ShowLastBooks(int amountLastBooks);
}
