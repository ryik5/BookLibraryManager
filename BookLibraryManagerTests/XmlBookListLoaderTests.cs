using System.Xml.Serialization;
using BookLibraryManager.Common;
using Moq;
using Xunit;

namespace BookLibraryManager.Tests;

/// <author>YR 2025-01-09</author>
public class XmlBookListLoaderTests
{
    [Fact]
    public void LoadLibrary_ValidFilePath_ReturnsLibrary()
    {
        // Arrange
        var filePath = "testLibrary.xml";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        var library = new LibraryManagerModel
        {
            Id = 1,
            BookList =
            [
                new() { Id = 1, Author = "Author1", Title = "Title1", TotalPages = 1 },
                new() { Id = 2, Author = "Author2", Title = "Title2", TotalPages = 2 }
            ]
        };

        var serializer = new XmlSerializer(typeof(LibraryAbstract));

        using (var writer = new StreamWriter(filePath))
        {
            serializer.Serialize(writer, library);
        }

        var loader = new XmlBookListLoader();

        // Act
        var result = loader.LoadLibrary(filePath, out var checkedlibrary);

        // Assert
        Xunit.Assert.True(result);
        Xunit.Assert.NotNull(checkedlibrary);
        Xunit.Assert.Equal(library.Id, checkedlibrary.Id);
        Xunit.Assert.Equal(library.BookList.Count, checkedlibrary.BookList.Count);
        Xunit.Assert.Equal(library.BookList[0].Id, checkedlibrary.BookList[0].Id);
        Xunit.Assert.Equal(library.BookList[0].Author, checkedlibrary.BookList[0].Author);
        Xunit.Assert.Equal(library.BookList[0].Title, checkedlibrary.BookList[0].Title);
        Xunit.Assert.Equal(library.BookList[0].TotalPages, checkedlibrary.BookList[0].TotalPages);

        // Cleanup
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    [Fact]
    public void LoadLibrary_ValidXmlFile_ReturnsTrue()
    {
        // Arrange
        var mockLibrary = new Mock<ILibrary>();
        var loader = new XmlBookListLoader();
        var pathToLibrary = "validLibrary.xml";
        var xmlContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<LibraryAbstract xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xsi:nil=\"true\" />";

        if (File.Exists(pathToLibrary))
        {
            File.Delete(pathToLibrary);
        }

        File.WriteAllText(pathToLibrary, xmlContent);

        // Act
        var result = loader.LoadLibrary(pathToLibrary, out It.Ref<ILibrary>.IsAny);

        // Assert
        Xunit.Assert.True(result);
        Xunit.Assert.IsAssignableFrom<ILibrary>(mockLibrary.Object);

        if (File.Exists(pathToLibrary))
        {
            File.Delete(pathToLibrary);
        }
    }

    [Fact]
    public void LoadLibrary_InvalidXmlFile_ReturnsFalse()
    {
        // Arrange
        var loader = new XmlBookListLoader();
        var pathToLibrary = "invalidLibrary.xml";
        var xmlContent = "<InvalidXml></InvalidXml>";

        if (File.Exists(pathToLibrary))
        {
            File.Delete(pathToLibrary);
        }
        File.WriteAllText(pathToLibrary, xmlContent);

        // Act
        var result = loader.LoadLibrary(pathToLibrary, out var library);

        // Assert
        Xunit.Assert.False(result);
        Xunit.Assert.Null(library);
        File.Delete(pathToLibrary);
    }

    [Fact]
    public void LoadLibrary_FileNotFound_ReturnsFalse()
    {
        // Arrange
        var loader = new XmlBookListLoader();
        var pathToLibrary = "nonExistentLibrary.xml";

        // Act
        var result = loader.LoadLibrary(pathToLibrary, out var library);

        // Assert
        Xunit.Assert.False(result);
        Xunit.Assert.Null(library);
    }
}
