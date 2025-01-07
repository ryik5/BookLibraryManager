
namespace BookLibraryManager.Models;

public class LibraryModel : LibraryAbstract, ILibrary
{
    public void AddBook(Book book)
    {
        BookList.Add(book);
    }

    public bool RemoveBook(Book book)
    {
        var searchBook = BookList.Find(b => b.Id == book.Id);
        return BookList.Remove(searchBook);
    }

    public void SortLibrary()
    {
        BookList = BookList.OrderBy(b => b.Author).ThenBy(b => b.Title).ToList();
    }

    public static LibraryModel GetNewLibrary(int idLibrary)
    {
        return new LibraryModel { Id = idLibrary, BookList = [] };
    }

    public List<Book> GetFirstBooks(int amountFirstBooks) => BookList.Take(amountFirstBooks).ToList();

    public List<Book> GetAllList() => BookList.ToList();

    public int AmountBooks => BookList.Count;

    public string ShowFistBooks(int amountFirstBooks)
    {
        var list = GetFirstBooks(amountFirstBooks).Select(bookSelector);
        return JoinStrings(list);
    }

    public string ShowLastBooks(int amountLastBooks)
    {
        var availableAmountBooks = AmountBooks < amountLastBooks ? AmountBooks : amountLastBooks;
        var list = BookList.Take(availableAmountBooks).Reverse().Select(bookSelector);

        return JoinStrings(list);
    }

    private Func<Book, string> bookSelector = b => $"{b.Id}. Author:{b.Author} - Title:{b.Title}";

    private string JoinStrings(IEnumerable<string> list) => string.Join(",\n", list);
}
