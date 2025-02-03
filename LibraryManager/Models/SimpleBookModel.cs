namespace LibraryManager.Models;

/// <summary>
/// Represents an example of the book model.
/// </summary>
/// <author>YR 2025-02-03</author>
public class SimpleBookModel
{
    public SimpleBookModel()
    {
    }

    public SimpleBookModel(int id, string author, string title, int year, int pages)
        => (Id, Author, Title, Year, Pages) = (id, author, title, year, pages);

    public int Id { get; private set; } = -1;

    public string Author { get; private set; } = string.Empty;

    public string Title { get; private set; } = string.Empty;

    public int Year { get; private set; } = 1970;

    public int Pages { get; private set; } = 20;
}