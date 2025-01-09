namespace BookLibraryManager.Models;
public interface ILibraryShower
{
    List<Book> GetFirstBooks(int amountFirstBooks);

    int AmountBooks { get; }

    string ShowFistBooks(int amountFirstBooks);
  
    string ShowLastBooks(int amountLastBooks);
}
