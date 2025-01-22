﻿using System.Collections.ObjectModel;
using System.Windows;

namespace BookLibraryManager.Common;

/// <summary>
/// Represents a model for managing a library of books.
/// </summary>
/// <author>YR 2025-01-09</author>
public class LibraryManagerModel : LibraryAbstract, ILibrary
{
    /// <summary>
    /// Creates a new library with the specified ID.
    /// </summary>
    /// <param name="idLibrary">The ID of the new library.</param>
    /// <returns>A new instance of LibraryModel.</returns>
    public static LibraryManagerModel CreateNewLibrary(int idLibrary)
    {
        return new LibraryManagerModel { Id = idLibrary, BookList = [] };
    }

    /// <summary>
    /// Adds a book to the library.
    /// </summary>
    /// <param name="book">The book to add.</param>
    public void AddBook(Book book)
    {
        BookList.Add(book);
    }

    /// <summary>
    /// Removes a book from the library.
    /// </summary>
    /// <param name="book">The book to remove.</param>
    /// <returns>True if the book was successfully removed; otherwise, false.</returns>
    public bool RemoveBook(Book book)
    {
        var searchBook = BookList.FirstOrDefault(b => b.Id == book.Id && b.Author == book.Author && b.Title == book.Title && b.TotalPages == book.TotalPages && b.PublishDate == book.PublishDate);

        return BookList.Remove(searchBook);
    }

    /// <summary>
    /// Sorts the book collection by author and then by title.
    /// </summary>
    public void SortLibrary()
    {
        BookList = new ObservableCollection<Book>(BookList.OrderBy(b => b.Author).ThenBy(b => b.Title));
    }

    /// <summary>
    /// Finds books in the library by a specified book element.
    /// </summary>
    /// <param name="library">The library to search in.</param>
    /// <param name="bookElement">The element of the book to search by.</param>
    /// <param name="partElement">The value of the element to search for.</param>
    /// <returns>A list of books that match the search criteria.</returns>
    public List<Book> FindBooksByBookElement(ILibrary library, BookElementsEnum bookElement, object partElement)
    {
        List<Book> result;
        IEnumerable<Book> tmpResult = new List<Book>();
        var strElement = partElement?.ToString();

        Application.Current.Dispatcher.Invoke(new Action(() =>
        {
            switch (bookElement)
            {
                case BookElementsEnum.Author:
                    tmpResult = FindBooksByAuthor(library, strElement);
                    break;
                case BookElementsEnum.Title:
                    tmpResult = FindBooksByTitle(library, strElement);
                    break;
                case BookElementsEnum.TotalPages:
                    tmpResult = FindBooksByTotalPages(library, strElement);
                    break;
                case BookElementsEnum.PublishDate:
                    tmpResult = FindBooksByPublishDate(library, strElement);
                    break;
            }
        }));
        result = tmpResult?.ToList() ?? new List<Book>();

        return result;
    }

    /// <summary>
    /// Finds books in the library by author.
    /// </summary>
    /// <param name="library">The library to search in.</param>
    /// <param name="strElement">The author to search for.</param>
    /// <returns>An enumerable collection of books that match the search criteria.</returns>
    public IEnumerable<Book> FindBooksByAuthor(ILibrary library, string? strElement)
        => (IsNotNullOrEmpty(strElement)) ? library.BookList.Where(b => b.Author.Contains(strElement, StringComparison.OrdinalIgnoreCase)) : [];

    /// <summary>
    /// Finds books in the library by title.
    /// </summary>
    /// <param name="library">The library to search in.</param>
    /// <param name="strElement">The title to search for.</param>
    /// <returns>An enumerable collection of books that match the search criteria.</returns>
    public IEnumerable<Book> FindBooksByTitle(ILibrary library, string? strElement)
        => (IsNotNullOrEmpty(strElement)) ? library.BookList.Where(b => b.Title.Contains(strElement, StringComparison.OrdinalIgnoreCase)) : [];

    /// <summary>
    /// Finds books in the library by total pages.
    /// </summary>
    /// <param name="library">The library to search in.</param>
    /// <param name="strElement">The total pages to search for.</param>
    /// <returns>An enumerable collection of books that match the search criteria.</returns>
    public IEnumerable<Book> FindBooksByTotalPages(ILibrary library, string? strElement)
        => IsParseable(strElement, out var intElement) ? library.BookList.Where(b => b.TotalPages == intElement) : [];

    /// <summary>
    /// Finds books in the library by publish date.
    /// </summary>
    /// <param name="library">The library to search in.</param>
    /// <param name="strElement">The publish date to search for.</param>
    /// <returns>An enumerable collection of books that match the search criteria.</returns>
    public IEnumerable<Book> FindBooksByPublishDate(ILibrary library, string? strElement)
        => IsParseable(strElement, out var intElement) ? library.BookList.Where(b => b.PublishDate == intElement) : [];

    /// <summary>
    /// Retrieves a collection of the first specified number of books from the library.
    /// </summary>
    /// <param name="amountFirstBooks">The number of books to retrieve.</param>
    /// <returns>A collection of the first books.</returns>
    public List<Book> GetFirstBooks(int amountFirstBooks) => BookList.Take(amountFirstBooks).ToList();

    /// <summary>
    /// Retrieves all books in the library.
    /// </summary>
    /// <returns>A collection of all books.</returns>
    public List<Book> GetAllBooks() => new(BookList);

    /// <summary>
    /// Gets the total number of books in the library.
    /// </summary>
    public int NumberOfBooks => BookList.Count;

    /// <summary>
    /// Returns a string representation of the full library.
    /// </summary>
    /// <returns>A string that represents the library.</returns>
    public override string ToString() => $"{Id}-{string.Join(",", BookList.Select(b => b))}";

    #region private methods
    /// <summary>
    /// Determines whether the specified string can be parsed to an integer.
    /// </summary>
    /// <param name="strElement">The string to parse.</param>
    /// <param name="intElement">The parsed integer value.</param>
    /// <returns>True if the string can be parsed to an integer; otherwise, false.</returns>
    private bool IsParseable(string? strElement, out int intElement) => int.TryParse(strElement, out intElement);

    /// <summary>
    /// Determines whether the specified string is not null or empty.
    /// </summary>
    /// <param name="strElement">The string to check.</param>
    /// <returns>True if the string is not null or empty; otherwise, false.</returns>
    private bool IsNotNullOrEmpty(string? strElement) => !string.IsNullOrEmpty(strElement);
    #endregion
}
