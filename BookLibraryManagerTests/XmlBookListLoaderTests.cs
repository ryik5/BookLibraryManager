using System.Collections.ObjectModel;
using System.Xml.Serialization;
using BookLibraryManager.Common;
using Xunit;

namespace BookLibraryManager.Tests;

/// <author>YR 2025-01-09</author>
public class XmlBookListLoaderTests
{
    [Fact]
    public void LoadLibrary_ValidFilePath_ReturnsLibrary()
    {
        // Arrange
        var library = new LibraryManagerModel
        {
            Id = 1,
            BookList = new ObservableCollection<Book>
            {
                new() { Id = 1, Author = "Author1", Title = "Title1", PageNumber = 1 },
                new() { Id = 2, Author = "Author2", Title = "Title2", PageNumber = 2 }
            }
        };

        var serializer = new XmlSerializer(typeof(LibraryAbstract));
        var filePath = "testLibrary.xml";

        using (var writer = new StreamWriter(filePath))
        {
            serializer.Serialize(writer, library);
        }

        var loader = new XmlBookListLoader();

        // Act
        var result = loader.LoadLibrary(filePath);

        // Assert
        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal(library.Id, result.Id);
        Xunit.Assert.Equal(library.BookList.Count, result.BookList.Count);
        Xunit.Assert.Equal(library.BookList[0].Id, result.BookList[0].Id);
        Xunit.Assert.Equal(library.BookList[0].Author, result.BookList[0].Author);
        Xunit.Assert.Equal(library.BookList[0].Title, result.BookList[0].Title);
        Xunit.Assert.Equal(library.BookList[0].PageNumber, result.BookList[0].PageNumber);

        // Cleanup
        File.Delete(filePath);
    }

    [Fact]
    public void LoadLibrary_InvalidFilePath_ThrowsFileNotFoundException()
    {
        // Arrange
        var loader = new XmlBookListLoader();
        var filePath = "nonExistentFile.xml";

        // Act & Assert
        Xunit.Assert.Throws<FileNotFoundException>(() => loader.LoadLibrary(filePath));
    }
}
