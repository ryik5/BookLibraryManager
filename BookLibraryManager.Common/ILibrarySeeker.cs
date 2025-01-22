namespace BookLibraryManager.Common;

/// <author>YR 2025-01-22</author>
public interface ILibrarySearcher
{

    List<Book> FindBooksByBookElement(ILibrary library, BookElementsEnum bookElement, object partOfElement);
}
