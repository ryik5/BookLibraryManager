namespace BookLibraryManager.Models;
public interface ILibraryShower
{
    List<Book> GetFirstBooks(int amountFirstBooks);

    List<Book> GetAllList();

    int AmountBooks { get; }

    string ShowFistBooks(int amountFirstBooks);
    string ShowLastBooks(int amountLastBooks);
}
